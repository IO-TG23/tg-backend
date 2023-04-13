using FluentValidation;

namespace TG.Backend.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(validationException.Errors);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 503;
            await context.Response.WriteAsync(exception.Message);
        }
    }
}