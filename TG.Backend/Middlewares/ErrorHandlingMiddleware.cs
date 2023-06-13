namespace TG.Backend.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //try
        //{
        //    await next.Invoke(context);
        //}
        //catch (NotFoundException notFoundException)
        //{
        //    context.Response.StatusCode = StatusCodes.Status404NotFound;
        //    await context.Response.WriteAsync($"Content with Id = ${notFoundException.Id} was not found");
        //}
        //catch (ValidationException validationException)
        //{
        //    context.Response.StatusCode = StatusCodes.Status400BadRequest;
        //    await context.Response.WriteAsJsonAsync(validationException.Errors);
        //}
        //catch (Exception exception)
        //{
        //    context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        //    await context.Response.WriteAsync(exception.Message);
        //}

        return Task.CompletedTask;
    }
}