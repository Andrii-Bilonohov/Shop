using Domain.Enums;

namespace Application.Abstractions.Services.JWT
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email, Role role);
    }
}
