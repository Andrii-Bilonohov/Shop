using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Application.Options;

namespace Infrastructure.Middlewars;

public sealed class ListenToOnlyGatewayApiMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GatewayOptions _options;
    private readonly IExceptionLogger? _logger;

    public ListenToOnlyGatewayApiMiddleware(RequestDelegate next, IOptions<GatewayOptions> options, IExceptionLogger? logger = null)
    {
        _next = next;
        _options = options.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsPublicEndpoint(context.Request.Path))
        {
            await _next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var headerValue))
        {
            await DenyAccess(context, $"Missing header '{_options.HeaderName}'.");
            return;
        }
        
        var incoming = headerValue.ToString();
        
        Console.WriteLine("Incoming header: " + incoming);
        Console.WriteLine("Expected secret: " + _options.Secret);
        
        if (string.IsNullOrWhiteSpace(incoming) || !SecretsEqual(incoming, _options.Secret))
        {
            await DenyAccess(context, $"Invalid gateway secret header '{_options.HeaderName}'.");
            return;
        }

        await _next(context);
    }

    private static bool IsPublicEndpoint(PathString path)
        => path.StartsWithSegments("/health");

    private static bool SecretsEqual(string a, string b)
    {
        if (a is null || b is null) return false;

        var aBytes = Encoding.UTF8.GetBytes(a.Trim());
        var bBytes = Encoding.UTF8.GetBytes(b.Trim());

        return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
    }

    private async Task DenyAccess(HttpContext context, string reason)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "application/json";

        _logger?.Log(
            new UnauthorizedAccessException(reason),
            "Blocked non-gateway request. IP: {IP}, Method: {Method}, Path: {Path}, RequiredHeader: {Header}",
            context.Connection.RemoteIpAddress?.ToString(),
            context.Request.Method,
            context.Request.Path,
            _options.HeaderName
        );

        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = "Access denied. This service is accessible only via API Gateway.",
            requiredHeader = _options.HeaderName
        }));
    }
}
