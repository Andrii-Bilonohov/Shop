namespace Application.DTOs.Payments.Responses;

public record PaymentStatusResponse(
    Guid Id,
    string Status
);