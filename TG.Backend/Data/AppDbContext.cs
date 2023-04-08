using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TG.Backend.Data
{
    /// <summary>
    /// EF Core Context dla aplikacji - mozna dodawac swoje DbSety
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
