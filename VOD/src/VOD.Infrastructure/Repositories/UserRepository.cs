namespace VOD.Infrastructure.Repositories
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using VOD.Domain.Entities;
    using VOD.Domain.Repositories;

    public class UserRepository : IUserRepository
    {
        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        private readonly SignInManager<User> _signInManager;

        private readonly UserManager<User> _userManager;

        private string RegisteredUser { get; set; } = "RegisteredUser";

        public async Task<bool> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(email, password, false, false);
            
            return result.Succeeded;
        }

        public async Task<User> GetByEmailAsync(string requestEmail, CancellationToken cancellationToken = default)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == requestEmail, cancellationToken);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(string requestEmail, CancellationToken cancellationToken = default)
        {
            return await _userManager.GetRolesAsync(await this.GetByEmailAsync(requestEmail, cancellationToken));
        }

        public async Task<bool> SignUpAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            user.CreationDate = DateTimeOffset.Now;
            IdentityResult result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return false;
            }
            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, RegisteredUser);

            return result.Succeeded && roleResult.Succeeded;
        }

        //public async Task<bool> EditUserAsync(User user, CancellationToken cancellationToken = default)
        //{
        //    user.ModificationDate = DateTimeOffset.Now;
        //    IdentityResult result = await _userManager.;

        //    return result.Succeeded;
        //}
    }
}
