using System.Security.Claims;
using Microsoft.Extensions.Options;
using Presentation.Options;

namespace Presentation.Middlewares;

public class AddUserHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GatewayOptions _gateway;

    public AddUserHeadersMiddleware(RequestDelegate next, IOptions<GatewayOptions> gatewayOptions)
    {
        _next = next;
        _gateway = gatewayOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.Headers[_gateway.HeaderName] = _gateway.Secret;
        
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var token = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(token))
            {
                context.Request.Headers["Authorization"] = token;
            }

            var userId = context.User.FindFirst("sub")?.Value;
            if (!string.IsNullOrEmpty(userId))
                context.Request.Headers["X-UserId"] = userId;

            var roles = context.User.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .Distinct()
                .ToArray();

            if (roles.Length > 0)
                context.Request.Headers["X-Roles"] = string.Join(",", roles);
        }

        await _next(context);
    }
}

public static class AddUserHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseAddUserHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<AddUserHeadersMiddleware>();
}