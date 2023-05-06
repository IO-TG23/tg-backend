using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace TG.Backend.Filters
{
    /// <summary>
    /// Filtr pobierający email z headera autoryzacyjnego
    /// </summary>
    public class GetCurrentUserFromTheHeaderFilter : IAsyncActionFilter
    {
        private readonly JwtSecurityTokenHandler _handler;

        public GetCurrentUserFromTheHeaderFilter(JwtSecurityTokenHandler handler)
        {
            _handler = handler;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            StringValues authHeader = context.HttpContext.Request.Headers.Authorization;

            JwtSecurityToken? token = _handler.ReadToken(authHeader.ToString().Split(" ")[1]) as JwtSecurityToken;

            string? email = token?.Claims.FirstOrDefault()?.Value;

            context.HttpContext.Items["email"] = email;

            await next();
        }
    }
}
