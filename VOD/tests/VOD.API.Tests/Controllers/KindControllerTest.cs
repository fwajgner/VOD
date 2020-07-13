namespace VOD.API.Tests.Controllers
{ 
    using AutoFixture;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Requests.Kind;
    using VOD.Domain.Responses;
    using VOD.Fixtures;
    using Xunit;

    public class KindControllerTest : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        #region Constructor, properties

        public KindControllerTest(InMemoryApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        private InMemoryApplicationFactory<Startup> Factory { get; }

        private Fixture GenData { get; set; } = new Fixture();

        private string Url { get; } = "/api/v1/kinds";

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
            var responseEntity = JsonConvert.DeserializeObject<PaginatedResponseModel<KindResponse>>(responseContent);
            Assert.Equal(pageIndex, responseEntity.PageIndex);
            Assert.Equal(pageSize, responseEntity.PageSize);
            Assert.Equal(pageSize, responseEntity.Data.Count());
            Assert.NotNull(responseEntity.Data.First().Name);
        }

        [Theory]
        [InlineData("6fa1bc43-ee6c-4d60-a2c8-9eb1a0494fd5")]
        public async Task GetById_should_return_spiecified_kind(string id)
        {
            //Arrange
            HttpClient client = Factory.CreateClient();

            //Act
            HttpResponseMessage response = await client.GetAsync($"{Url}/{id}");

            //Assert
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            Kind responseEntity = JsonConvert.DeserializeObject<Kind>(responseContent);

            responseEntity.Should().NotBeNull();
            Assert.NotNull(responseEntity.Name);
        }

        [Fact]
        public async Task Add_should_create_new_kind()
        {
            //Arrange
            AddKindRequest request = new AddKindRequest() { KindName = "new kind" };

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
