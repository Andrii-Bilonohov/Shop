namespace Application.DTOs.Requests.Auth
{
    public record LoginLocal
    (
        string Email, 
        string Password
    );
}