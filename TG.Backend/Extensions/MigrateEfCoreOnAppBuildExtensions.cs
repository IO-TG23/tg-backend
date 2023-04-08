namespace TG.Backend.Extensions
{
    public static class MigrateEfCoreOnAppBuildExtensions
    {
        public static async Task MigrateEfCoreOnAppBuild(this IServiceProvider serviceProvider)
        {
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            using AppDbContext? db = scope.ServiceProvider.GetService<AppDbContext>();
            await db!.Database.MigrateAsync();
        }
    }
}
