using Domain.Enums;

namespace Application.DTOs.Requests.Auth
{
    public record RegisterLocal
    (
        string Email,
        string FirstName,
        string LastName,
        string Password,
        Role Role
    );
}
