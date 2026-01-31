using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeRolesAttribute : Attribute, IAsyncActionFilter
{
    private readonly string[] _roles;

    public AuthorizeRolesAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var rolesHeader = context.HttpContext.Request.Headers["X-Roles"].FirstOrDefault();
        var userRoles = rolesHeader?.Split(',') ?? Array.Empty<string>();

        // Проверяем, есть ли нужная роль
        if (!_roles.Any(r => userRoles.Contains(r)))
        {
            context.Result = new ForbidResult(); // 403
            return;
        }

        await next();
    }
}