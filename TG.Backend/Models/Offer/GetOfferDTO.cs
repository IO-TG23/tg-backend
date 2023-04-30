namespace TG.Backend.Models.Offer;

public class GetOfferDTO
{
    public required Guid Id { get; set; }
    public required string VehicleName { get; set; }
    public required string VehicleDescription { get; set; }
    public IEnumerable<Guid> BlobIds { get; set; } = new List<Guid>();
}