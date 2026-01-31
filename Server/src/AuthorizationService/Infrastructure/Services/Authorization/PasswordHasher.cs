using Application.Abstractions.Services.JWT;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Authorization
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<object> _hasher = new();

        public string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string password, string passwordHash)
        {
            var result = _hasher.VerifyHashedPassword(
                null!,
                passwordHash,
                password
            );

            return result == PasswordVerificationResult.Success;
        }
    }
}