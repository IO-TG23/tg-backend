namespace TG.Backend.Extensions
{
    /// <summary>
    /// Implementacja automatycznego aplikowania migracji (nie ma potrzeby robienia Update-Database)
    /// </summary>
    public static class MigrateEfCoreOnAppBuildExtensions
    {
        public static async Task MigrateEfCoreOnAppBuild(this IServiceProvider serviceProvider)
        {
            await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
            await using AppDbContext? db = scope.ServiceProvider.GetService<AppDbContext>();
            await db!.Database.MigrateAsync();
        }
    }
}
