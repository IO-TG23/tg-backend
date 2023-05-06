namespace TG.Backend.Models.Client
{
    public class CreateClientDTO
    {
        public required Guid AppUserId { get; set; }
        public required AppUser AppUser { get; set; }
    }
}
