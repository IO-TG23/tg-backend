using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TG.Backend.Data
{
    /// <summary>
    /// EF Core Context dla aplikacji - mozna dodawac swoje DbSety
    /// </summary>
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Blob> Blobs => Set<Blob>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}