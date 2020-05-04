namespace VOD.Domain.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using VOD.Domain.Requests.User;
    using VOD.Domain.Responses;

    public interface IUserService
    {
        Task<UserResponse> GetUserAsync(GetUserRequest request,
            CancellationToken cancellationToken = default);

        Task<UserResponse> SignUpAsync(SignUpRequest request,
            CancellationToken cancellationToken = default);

        Task<TokenResponse> SignInAsync(SignInRequest request,
            CancellationToken cancellationToken = default);
    }
}
