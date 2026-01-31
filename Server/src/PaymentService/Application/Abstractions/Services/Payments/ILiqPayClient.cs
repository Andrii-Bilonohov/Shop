using Application.DTOs.Payments.Requests;
using Application.DTOs.Payments.Responses;

namespace Application.Abstractions.Services.Payments;

public interface ILiqPayClient
{
    LiqPayForm CreatePayment(LiqPayPayment request);
    bool VerifySignature(string data, string signature);
}
