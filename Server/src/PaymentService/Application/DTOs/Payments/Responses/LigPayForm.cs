namespace Application.DTOs.Payments.Responses;

public record LiqPayForm(
    string Data,
    string Signature
);