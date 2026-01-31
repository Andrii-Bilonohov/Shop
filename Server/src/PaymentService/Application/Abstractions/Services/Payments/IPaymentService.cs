using Application.Contracts.Base;
using Application.DTOs.Payments.Requests;
using Application.DTOs.Payments.Responses;

namespace Application.Abstractions.Services.Payments;

public interface IPaymentService
{
    Task<Response<LiqPayForm>> PayOrderAsync(Guid orderId, BuyOrder request, CancellationToken ct);
    ValueTask<Response<PaymentStatusResponse>> GetPaymentStatusAsync(Guid orderId, CancellationToken ct);
    Task<Information> UpdateStatusAsync(Guid id, UpdatePaymentStatus request, CancellationToken ct);
}