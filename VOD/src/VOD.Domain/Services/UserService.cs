namespace VOD.Domain.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VOD.Domain.Repositories;
    using VOD.Domain.Requests.User;
    using VOD.Domain.Responses;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;
    using VOD.Domain.Configurations;
    using VOD.Domain.Entities;
    using System.IdentityModel.Tokens.Jwt;
    using System.Text;
    using System.Security.Claims;

    public class UserService : IUserService
    {
        public UserService(IUserRepository userRepository, IOptions<AuthenticationSettings> authenticationSettings)
        {
            _userRepository = userRepository;
            _authenticationSettings = authenticationSettings.Value;
        }

        private readonly AuthenticationSettings _authenticationSettings;

        private readonly IUserRepository _userRepository;

        public async Task<UserResponse> GetUserAsync(GetUserRequest request, CancellationToken cancellationToken = default)
        {
            User response = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            return new UserResponse
            {
                Name = response.Name,
                Email = response.Email
            };
        }

        public async Task<TokenResponse> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
        {
            bool response = await _userRepository.AuthenticateAsync(request.Email, request.Password, cancellationToken);

            return response == false ? null : new TokenResponse
            {
                Token = GenerateSecurityToken(request)
            };
        }

        public async Task<UserResponse> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
        {
            User user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                Name = request.Name
            };

            bool result = await _userRepository.SignUpAsync(user, request.Password, cancellationToken);

            return !result ? null : new UserResponse
            {
                Name = request.Name,
                Email = request.Email
            };
        }

        private string GenerateSecurityToken(SignInRequest request)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_authenticationSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, request.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(_authenticationSettings.ExpirationDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
