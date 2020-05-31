namespace VOD.Fixtures
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class UsersContextFactory
    {
        public UsersContextFactory()
        {
            _passwordHasher = new PasswordHasher<User>();

            _users = new List<User>();

            User user = new User
            {
                Id = "test_id",
                Email = "filip.wajgner@example.com",
                Name = "Filip Wajgner"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, "P@$$w0rd");

            _users.Add(user);
        }

        private readonly PasswordHasher<User> _passwordHasher;

        private readonly IList<User> _users;

        public IUserRepository InMemoryUserManager => GetInMemoryUserManager();

        private IUserRepository GetInMemoryUserManager()
        {
            Mock<IUserRepository> fakeUserService = new Mock<IUserRepository>();

            fakeUserService.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, string password, CancellationToken token) =>
                {
                    User user = _users.FirstOrDefault(x => x.Email == email);

                    if (user == null) return false;

                    PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

                    return result == PasswordVerificationResult.Success;
                });

            fakeUserService.Setup(x => x.GetByEmailAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, CancellationToken token) => _users.First(x => x.Email == email));

            fakeUserService.Setup(x => x.GetUserRolesAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string email, CancellationToken token) => new List<string>() { "Test Role" });

            fakeUserService.Setup(x => x.SignUpAsync(It.IsAny<User>(), It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((User user, string password, CancellationToken token) =>
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                    _users.Add(user);
                    return true;
                });

            return fakeUserService.Object;
        }
    }
}
