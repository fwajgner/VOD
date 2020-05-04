namespace VOD.API.Tests.Controllers
{
    using FluentAssertions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using VOD.Domain.Requests.User;
    using VOD.Domain.Responses;
    using VOD.Fixtures;
    using Xunit;

    public class UserControllerTests : IClassFixture<InMemoryApplicationFactory<Startup>>
    {
        public UserControllerTests(InMemoryApplicationFactory<Startup> factory)

        {
            _factory = factory;
        }

        private readonly InMemoryApplicationFactory<Startup> _factory;

        [Theory]
        [InlineData("/api/v1/users/auth")]
        public async Task Sign_in_should_retrieve_a_token(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            SignInRequest request = new SignInRequest
            {
                Email = "filip.wajgner@example.com",
                Password = "P@$$w0rd"
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PostAsync(url, httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("/api/v1/users/auth")]
        public async
        Task Sign_in_should_retrieve_bad_request_with_invalid_password(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();
            SignInRequest request = new SignInRequest
            {
                Email = "filip.wajgner@example.com",
                Password = "NotValidPWD"
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json");

            // Act
            HttpResponseMessage response = await client.PostAsync(url, httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData("/api/v1/users")]
        public async Task Get_with_authorized_user_should_retrieve_the_right_user(string url)
        {
            // Arrange - authorization
            HttpClient client = _factory.CreateClient();

            SignInRequest signInRequest = new SignInRequest
            {
                Email = "filip.wajgner@example.com",
                Password = "P@$$w0rd"
            };

            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(signInRequest), 
                Encoding.UTF8, "application/json");

            // Act - authorization
            HttpResponseMessage response = await client.PostAsync(url + "/auth", httpContent);
            string responseContent = await response.Content.ReadAsStringAsync();

            // Assert - authorization
            response.EnsureSuccessStatusCode();

            // Arrange - get user
            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.Token);

            // Act - - get user
            HttpResponseMessage restrictedResponse = await client.GetAsync(url);

            // Assert - get user
            restrictedResponse.EnsureSuccessStatusCode();
            restrictedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
