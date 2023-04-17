namespace TG.Backend.Data;

public class Offer : Entity
{
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPhoneNumber { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public Guid VehicleId { get; set; }
}