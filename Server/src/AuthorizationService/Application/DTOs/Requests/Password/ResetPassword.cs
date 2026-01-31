namespace Application.DTOs.Requests.Password
{
    public record ResetPassword(string Email, string Token, string NewPassword);
}
