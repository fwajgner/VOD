namespace VOD.Domain.Tests.Services
{
    using FluentAssertions;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using VOD.Domain.Configurations;
    using VOD.Domain.Requests.User;
    using VOD.Domain.Responses;
    using VOD.Domain.Services;
    using VOD.Fixtures;
    using Xunit;

    public class UserServiceTest : IClassFixture<UsersContextFactory>
    {
        public UserServiceTest(UsersContextFactory usersContextFactory)
        {
            _userService = new UserService(usersContextFactory.InMemoryUserManager, Options.Create(
             new AuthenticationSettings
             {
                 Secret = "Very Secret key-word to match",
                 ExpirationDays = 7
             }));
        }

        private readonly IUserService _userService;

        [Fact]
        public async Task SignIn_with_invalid_user_should_return_a_valid_token_response()
        {
            //Arrange
            SignInRequest signInRequest = new SignInRequest
            {
                Email = "invalid.user",
                Password = "invalid_password"
            };

            //Act
            TokenResponse result = await _userService.SignInAsync(signInRequest);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task SignIn_with_valid_user_should_return_a_valid_token_response()
        {
            //Arrange
            SignInRequest signInRequest = new SignInRequest
            {
                Email = "filip.wajgner@example.com",
                Password = "P@$$w0rd"
            };

            //Act
            TokenResponse result = await _userService.SignInAsync(signInRequest);

            //Assert
            result.Token.Should().NotBeEmpty();
        }
    }
}
