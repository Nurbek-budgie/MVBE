using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Configurations.Extentions;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace API.Configurations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class AuthorizeRoleAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    private readonly ERoles[] _roles;

    public AuthorizeRoleAttribute(params ERoles[] roles)
    {
        _roles = roles;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
       var request  = context.HttpContext.Request;
       var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

       if (string.IsNullOrEmpty(token))
       {
           context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
           return;
       }

       
       // TODO check token validation parameters
       try
       {
           var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
           
           var roleFromToken = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role || x.Type == "role")?.Value.ToLower();
           var roleFromEnum = Enum.GetValues(typeof(ERoles))
               .Cast<ERoles>()
               .Where(role => _roles.Contains(role)) 
               .Select(role => role.GetDescription())
               .ToArray();

           if (string.IsNullOrEmpty(roleFromToken) || !roleFromEnum.Contains(roleFromToken))
           {
               context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
           }
           
           await next();
       }
       catch (SecurityTokenException ex)
       {
           context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
       }
       catch (Exception ex)
       {
           context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
       }
    }
}