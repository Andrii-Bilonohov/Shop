using Application.Contracts.Auth;
using Application.DTOs.Requests.Auth;
using Application.DTOs.Requests.Password;

namespace Application.Abstractions.Services.Authorization
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthGoogleAsync(string idToken, CancellationToken ct);
        Task<AuthResponse> AuthLocalAsync(LoginLocal login, CancellationToken ct);
        Task<AuthResponse> RegisterLocalAsync(RegisterLocal register, CancellationToken ct);
        Task<AuthResponse> ForgotPasswordAsync(ForgotPassword request, CancellationToken ct);
        Task<AuthResponse> ResetPasswordAsync(ResetPassword request, CancellationToken ct);
    }
}
