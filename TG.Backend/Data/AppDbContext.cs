using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TG.Backend.Models.Auth;

namespace TG.Backend.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
