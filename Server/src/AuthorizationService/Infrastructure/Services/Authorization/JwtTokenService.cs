using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Services.JWT;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Authorization
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public int ExpiryMinutes { get; set; } = 60;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        
        public JwtSettings() { }
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> options)
        {
            _settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public string GenerateToken(Guid userId, string email, Role role)
        {
            if (string.IsNullOrWhiteSpace(_settings.Secret))
                throw new InvalidOperationException("Jwt:Secret is empty");

            if (_settings.ExpiryMinutes <= 0)
                throw new InvalidOperationException("Jwt:ExpiryMinutes must be > 0");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(ClaimTypes.Role, role.ToString()), 
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: now.AddMinutes(_settings.ExpiryMinutes), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
