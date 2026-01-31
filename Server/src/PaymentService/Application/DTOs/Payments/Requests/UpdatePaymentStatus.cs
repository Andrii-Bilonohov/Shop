using Domain.Enums;

namespace Application.DTOs.Payments.Requests;

public record UpdatePaymentStatus
(
    PaymentStatus Status
);