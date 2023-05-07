namespace TG.Backend.Data
{
    public class Client : Entity
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public ICollection<Vehicle>? Vehicles { get; set; } = new List<Vehicle>();
    }
}
