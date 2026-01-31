using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class GatewayAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public string[] Roles { get; set; } = Array.Empty<string>();

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Request.Headers.ContainsKey("X-Api-Gateway"))
        {
            context.Result = new StatusCodeResult(403);
            return;
        }
        
        var rolesHeader = context.HttpContext.Request.Headers["X-Roles"].ToString();
        if (string.IsNullOrWhiteSpace(rolesHeader))
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        if (Roles.Length == 0) return;

        var userRoles = rolesHeader
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var ok = Roles.Any(required =>
            userRoles.Any(r => string.Equals(r, required, StringComparison.OrdinalIgnoreCase)));

        if (!ok)
            context.Result = new StatusCodeResult(403);
    }
}