namespace API.Services.Interfaces
{
    using Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserService : IService<ApplicationUser>
    {
        Task<IEnumerable<ApplicationUser>> ReadAllWithSubscription();

        Task<IEnumerable<ApplicationUser>> ReadAllWithoutSubscription();

        Task<ApplicationUser> LockAccountAsync(string userName);
    }
}
