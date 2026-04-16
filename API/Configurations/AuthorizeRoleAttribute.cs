using System.Security.Claims;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Configurations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class AuthorizeRoleAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    private readonly HashSet<string> _allowedRoles;

    public AuthorizeRoleAttribute(params ERoles[] roles)
    {
        _allowedRoles = roles.Select(r => r.ToString()).ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Inheriting from AuthorizeAttribute makes the JwtBearer middleware run
        // first, so HttpContext.User is populated from a *validated* token
        // (signature, issuer, audience, expiry all checked). Only check roles here.
        var user = context.HttpContext.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
            return;
        }

        var roleClaims = user.FindAll(ClaimTypes.Role).Concat(user.FindAll("role"));
        if (!roleClaims.Any(c => _allowedRoles.Contains(c.Value)))
        {
            context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            return;
        }

        await next();
    }
}
