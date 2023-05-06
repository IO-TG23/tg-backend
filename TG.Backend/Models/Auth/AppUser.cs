namespace TG.Backend.Models.Auth
{
    public class AppUser : IdentityUser
    {
        public Guid? ClientId { get; set; }
        public Data.Client? Client { get; set; }
    }
}
