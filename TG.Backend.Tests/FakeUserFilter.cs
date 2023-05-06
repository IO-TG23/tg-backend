using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TG.Backend.Tests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new []
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.Empty.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }));

        context.HttpContext.User = claimsPrincipal;
        await next();
    }
}