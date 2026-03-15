using Application.Abstractions.Repositories;
using Application.Abstractions.Services.Authorization;
using Application.Abstractions.Services.JWT;
using Application.Abstractions.Services.UnitOfWork;
using Application.Contracts.Auth;
using Application.DTOs.Requests.Auth;
using Application.DTOs.Requests.Password;
using Application.Options;
using Domain.Enums;
using Domain.Models;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IAuthRepository repository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, JwtSettings jwtSettings)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _unitOfWork = unitOfWork ??  throw new ArgumentNullException(nameof(unitOfWork));
            _jwtSettings =  jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
        }

        public async Task<AuthResponse> AuthGoogleAsync(string idToken, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> AuthLocalAsync(LoginLocal login, CancellationToken ct)
        {
            var user = await _repository.GetByAsync(u => u.Email == login.Email, ct);
            if (user is null)
                return new AuthResponse(Error: "User not found");

            var isValid = _passwordHasher.Verify(login.Password, user.PasswordHash!);
            if (!isValid)
                return new AuthResponse(Error: "Invalid password");

            string? token = null;
            if (!string.IsNullOrEmpty(_jwtSettings.Secret))
            {
                token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role);
            }

            return new AuthResponse(AccessToken: token);
        }

        public async Task<AuthResponse> RegisterLocalAsync(RegisterLocal register, CancellationToken ct)
        {
            var existing = await _repository.GetByAsync(u => u.Email == register.Email, ct);
            if (existing != null)
                return new AuthResponse(Error: "User already exists");

            var user = User.CreateLocal(
                email: register.Email,
                firstName: register.FirstName,
                lastName: register.LastName,
                passwordHash: _passwordHasher.Hash(register.Password),
                role: register.Role
            );

            _repository.Add(user);
            await _unitOfWork.SaveChangesAsync(ct);

            string? token = null;
            if (!string.IsNullOrEmpty(_jwtSettings.Secret))
            {
                token = _jwtTokenService.GenerateToken(user.Id, user.Email, user.Role);
            }

            return new AuthResponse(AccessToken: token);
        }

        public async Task<AuthResponse> ForgotPasswordAsync(ForgotPassword request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponse> ResetPasswordAsync(ResetPassword request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
