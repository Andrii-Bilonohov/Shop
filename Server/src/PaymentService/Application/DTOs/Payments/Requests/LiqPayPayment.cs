namespace Application.DTOs.Payments.Requests;

public record LiqPayPayment(
    string OrderId,
    decimal Amount,
    string Description
);