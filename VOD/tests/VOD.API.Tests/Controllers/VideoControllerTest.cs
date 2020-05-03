namespace VOD.API.Tests.Controllers
{
    using AutoFixture;
    using FluentAssertions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.API.Controllers;
    using VOD.Domain.Entities;
    using VOD.Domain.Requests.Video;
    using VOD.Domain.Responses;
    using VOD.Fixtures;
    using VOD.Fixtures.Data;
    using Xunit;

    public class VideoControllerTest : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        #region Constructor, properties

        public VideoControllerTest(InMemoryApplicationFactory<Startup> factory) 
        {
            this.Factory = factory;
        }

        private InMemoryApplicationFactory<Startup> Factory { get; }

        private string Url { get; } = "api/v1/videos";

        #endregion


        [Theory]
        [InlineData("?pageSize=1&pageIndex=0", 1, 0)]
        [InlineData("?pageSize=2&pageIndex=0", 2, 0)]
        [InlineData("?pageSize=1&pageIndex=1", 1, 1)]
        public async Task Get_should_return_success_(string queryParam, int pageSize, int pageIndex)
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync($"{Url}{queryParam}");

            //Assert
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var responseEntity = JsonConvert.DeserializeObject<PaginatedResponseModel<VideoResponse>>(responseContent);
            Assert.Equal(pageIndex, responseEntity.PageIndex);
            Assert.Equal(pageSize, responseEntity.PageSize);
            Assert.Equal(pageSize, responseEntity.Data.Count());
            Assert.NotNull(responseEntity.Data.First().AltTitle);
        }


        [Theory]
        [InlineData("eaa0B9d4-4A2d-496e-8a68-a36cd0758abb")]
        public async Task GetById_should_return_spiecified_video(string id)
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync($"{Url}/{id}");

            //Assert
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Video responseEntity = JsonConvert.DeserializeObject<Video>(responseContent);

            responseEntity.Should().NotBeNull();
            Assert.NotNull(responseEntity.AltTitle);
            responseEntity.KindId.Should().NotBeEmpty();
            responseEntity.GenreId.Should().NotBeEmpty();
            //Assert.False(responseEntity.IsInactive);
        }


        [Fact]
        public async Task Add_should_create_new_video()
        {
            //Arrange
            AddVideoRequest request = TestDataFactory.CreateAddVideoRequest();
            request.KindId = new Guid("6fa1bc43-ee6c-4d60-a2c8-9eb1a0494fd5");
            request.GenreId = new Guid("2af1ad8c-60df-4d40-a6c7-d68da49d006a");

            HttpClient client = Factory.CreateClient();
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request), 
                Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PostAsync($"{Url}", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();
            response.Headers.Location.Should().NotBeNull();
        }

        [Fact]
        public async Task Update_should_modify_video()
        {
            //Arrange
            EditVideoRequest request = TestDataFactory.CreateEditVideoRequest();
            request.KindId = new Guid("2fd37626-0b1d-4d51-a243-0bc4266a7a99");
            request.GenreId = new Guid("9dc0d6f2-a3d8-41a9-8393-d832c0b7a6e9");
            Guid Id = new Guid("eaa0B9d4-4A2d-496e-8a68-a36cd0758abb");

            HttpClient client = Factory.CreateClient();
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PutAsync($"{Url}/{Id}", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Video responseEntity = JsonConvert.DeserializeObject<Video>(responseContent);
            responseEntity.Should().BeEquivalentTo(request, o => o.Excluding(x => x.Id));
            Assert.Equal(Id, responseEntity.Id);
        }



        [Theory]
        [InlineData("d042d01a-1839-4558-ba94-ffc7f9237f45")]
        public async Task Delete_should_returns_no_content_when_called_with_right_id(string id)
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.DeleteAsync($"{Url}/{id}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        [Fact]
        public async Task Delete_should_returns_not_found_when_called_with_not_existing_id()
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.DeleteAsync($"{Url}/{Guid.NewGuid()}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
