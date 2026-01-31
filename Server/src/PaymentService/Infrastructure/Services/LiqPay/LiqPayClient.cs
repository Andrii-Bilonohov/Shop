using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Application.Abstractions.Services.Payments;
using Application.DTOs.Payments.Requests;
using Application.DTOs.Payments.Responses;
using Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.LiqPay;

public class LiqPayClient : ILiqPayClient
{
    private readonly LiqPayOptions _options;

    public LiqPayClient(IOptions<LiqPayOptions> options)
    {
        _options = options.Value;
    }

    public LiqPayForm CreatePayment(LiqPayPayment request)
    {
        var payload = new
        {
            public_key = _options.PublicKey,
            version = "3",
            action = "pay",
            amount = request.Amount,
            currency = "UAH",
            description = request.Description,
            order_id = request.OrderId,
            server_url = _options.CallbackUrl,
            result_url = _options.ResultUrl,
            language = "uk"
        };

        var json = JsonSerializer.Serialize(payload)
                   ?? throw new InvalidOperationException("LiqPay payload serialization failed");

        var data = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        var signature = CreateSignature(data);

        return new LiqPayForm(data, signature);
    }

    public bool VerifySignature(string data, string signature)
    {
        var expected = CreateSignature(data);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expected),
            Convert.FromBase64String(signature)
        );
    }

    private string CreateSignature(string data)
    {
        var raw = _options.PrivateKey + data + _options.PrivateKey;
        using var sha1 = SHA1.Create();
        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToBase64String(hash);
    }
}
