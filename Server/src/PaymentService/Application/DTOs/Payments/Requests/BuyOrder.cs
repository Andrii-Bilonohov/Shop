using Domain.Enums;

namespace Application.DTOs.Payments.Requests;

public record BuyOrder
(
    Guid UserId,
    PaymentMethod Method,
    decimal Amount
);