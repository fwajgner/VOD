namespace VOD.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;

    public interface IUserRepository
    {
        Task<bool> AuthenticateAsync(string email, string password,
            CancellationToken cancellationToken = default);

        Task<bool> SignUpAsync(User user, string password,
            CancellationToken cancellationToken = default);

        Task<User> GetByEmailAsync(string requestEmail,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> GetUserRolesAsync(string requestEmail,
            CancellationToken cancellationToken = default);
    }
}
