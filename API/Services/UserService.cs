namespace API.Services
{
    using API.Services.Interfaces;
    using Context;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserService : IUserService
    {
        public UserService(ApplicationDbContext context)
        {
            this.Db = context ?? throw new ArgumentNullException(nameof(context));
        }

        private ApplicationDbContext Db { get; set; }

        public Task<ApplicationUser> CreateAsync(ApplicationUser obj)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> LockAccountAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> ReadAllWithoutSubscription()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> ReadAllWithSubscription()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> ReadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> UpdateAsync(string id, ApplicationUser newObj)
        {
            throw new NotImplementedException();
        }
    }
}
