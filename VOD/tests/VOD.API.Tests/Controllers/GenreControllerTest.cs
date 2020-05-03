namespace VOD.API.Tests.Controllers
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;
    using AutoFixture;
    using Microsoft.AspNetCore.Mvc;
    using VOD.Fixtures;
    using System.Net.Http;
    using Newtonsoft.Json;
    using VOD.Domain.Responses;
    using VOD.Domain.Entities;
    using VOD.Domain.Requests.Genre;
    using System.Text;

    public class GenreControllerTest : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        #region Constructor, properties
        
        public GenreControllerTest(InMemoryApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        private InMemoryApplicationFactory<Startup> Factory { get; }

        private Fixture GenData { get; set; } = new Fixture();

        private string Url { get; } = "/api/v1/genres";

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
            var responseEntity = JsonConvert.DeserializeObject<PaginatedResponseModel<GenreResponse>>(responseContent);
            Assert.Equal(pageIndex, responseEntity.PageIndex);
            Assert.Equal(pageSize, responseEntity.PageSize);
            Assert.Equal(pageSize, responseEntity.Data.Count());
            Assert.NotNull(responseEntity.Data.First().Name);
        }

        [Theory]
        [InlineData("2af1ad8c-60df-4d40-a6c7-d68da49d006a")]
        public async Task GetById_should_return_spiecified_kind(string id)
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync($"{Url}/{id}");

            //Assert
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Genre responseEntity = JsonConvert.DeserializeObject<Genre>(responseContent);

            responseEntity.Should().NotBeNull();
            Assert.NotNull(responseEntity.Name);
        }

        [Fact]
        public async Task Add_should_create_new_genre()
        {
            //Arrange
            AddGenreRequest request = new AddGenreRequest() { GenreName = "new genre" };

            HttpClient client = Factory.CreateClient();
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await client.PostAsync($"{Url}", httpContent);

            //Assert
            response.EnsureSuccessStatusCode();
            response.Headers.Location.Should().NotBeNull();
        }
    }

}
