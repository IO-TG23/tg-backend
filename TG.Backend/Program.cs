using Lib.AspNetCore.ServerSentEvents;
using TG.Backend.Data.SSE;
using TG.Backend.Extensions;
using TG.Backend.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.Services.MigrateEfCoreOnAppBuild();
app.UseCors("AllowAnyone");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapServerSentEvents<NotificationsServerSentEventsService>("/sse");
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program
{
}