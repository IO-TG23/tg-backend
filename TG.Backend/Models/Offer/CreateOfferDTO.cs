namespace TG.Backend.Models.Offer;

public class CreateOfferDTO
{
    public required VehicleDTO Vehicle { get; set; }
    public decimal Price { get; set; }
    public required string Description { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPhoneNumber { get; set; }
}