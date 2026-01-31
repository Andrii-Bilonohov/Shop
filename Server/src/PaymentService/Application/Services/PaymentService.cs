using Application.Abstractions.Repositories;
using Application.Abstractions.Services.Payments;
using Application.Abstractions.Services.UnitOfWork;
using Application.Contracts.Base;
using Application.DTOs.Payments.Requests;
using Application.DTOs.Payments.Responses;
using Domain.Enums;
using Domain.Models;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly ILiqPayClient _liqPayClient;
    private readonly IUnitOfWork _unitOfWork;

    public PaymentService(IPaymentRepository paymentRepository, ILiqPayClient liqPayClient, IUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _liqPayClient = liqPayClient;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<LiqPayForm>> PayOrderAsync(Guid orderId, BuyOrder request, CancellationToken ct)
    {
        try
        {
            if (orderId == Guid.Empty)
                return new Response<LiqPayForm>("OrderId is invalid");

            var payment = new Payment(
                request.UserId,
                orderId,
                request.Method
            );

            _paymentRepository.Add(payment);
            await _unitOfWork.SaveChangesAsync(ct);

            var form = _liqPayClient.CreatePayment(new LiqPayPayment(
                payment.Id.ToString(),
                request.Amount,
                $"Order #{orderId}"
            ));

            payment.MarkPending(payment.Id.ToString());
            await _unitOfWork.SaveChangesAsync(ct);

            return new Response<LiqPayForm>(Data: form);
        }
        catch (ArgumentException ex)
        {
            return new Response<LiqPayForm>(Error: ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return new Response<LiqPayForm>(Error: ex.Message);
        }
        catch (OperationCanceledException)
        {
            return new Response<LiqPayForm>("Operation was cancelled");
        }
        catch (Exception ex)
        {
            return new Response<LiqPayForm>("Unexpected error while creating payment");
        }
    }

    public async ValueTask<Response<PaymentStatusResponse>> GetPaymentStatusAsync(Guid id, CancellationToken ct)
    {
        try
        {
            if (id == Guid.Empty)
                return new Response<PaymentStatusResponse>("OrderId is invalid");

            var payment = await _paymentRepository.GetByIdAsync(id, ct);

            if (payment is null)
                return new Response<PaymentStatusResponse>("Payment not found");

            return new Response<PaymentStatusResponse>(
                Data: new PaymentStatusResponse(
                    payment.Id,
                    payment.Status.ToString()
                )
            );
        }
        catch (ArgumentException ex)
        {
            return new Response<PaymentStatusResponse>(Error: ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return new Response<PaymentStatusResponse>(Error: ex.Message);
        }
        catch (OperationCanceledException)
        {
            return new Response<PaymentStatusResponse>("Operation was cancelled");
        }
        catch (Exception ex)
        {
            return new Response<PaymentStatusResponse>("Unexpected error while fetching payment status");
        }
    }

    public async Task<Information> UpdateStatusAsync(Guid id, UpdatePaymentStatus request, CancellationToken ct)
    {
        try
        {
            var payment = await _paymentRepository.GetByIdAsync(id, ct);
            if (payment is null)
                return new Information("Payment not found");
            
            if (payment.IsFinal)
                return new Information("Cannot update final payment");
            
            
            switch (request.Status)
            {
                case PaymentStatus.Succeeded:
                    payment.MarkSucceeded();
                    break;
                case PaymentStatus.Failed:
                    payment.MarkFailed();
                    break;
                case PaymentStatus.Canceled:
                    payment.MarkCanceled();
                    break;
                case PaymentStatus.Refunded:
                    payment.MarkRefunded();
                    break;
                default:
                    return new Information("Cannot set this status");
            }

            payment.Touch();
            await _unitOfWork.SaveChangesAsync(ct);

            return new Information(Id: id, Message: "Payment status updated successfully");
        }
        catch (Exception ex)
        {
            return new Information($"Unexpected error: {ex.Message}");
        }
    }
}
