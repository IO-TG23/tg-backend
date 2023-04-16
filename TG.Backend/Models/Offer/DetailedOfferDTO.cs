namespace TG.Backend.Models.Offer;

public class DetailedOfferDTO
{
    public required decimal Price { get; set; }
    public required string OfferDescription { get; set; }
    public required string ContactEmail { get; set; }
    public required string ContactPhoneNumber { get; set; }
    public required string VehicleName { get; set; }
    public required string VehicleDescription { get; set; }
    public required int ProductionStartYear { get; set; }
    public int? ProductionEndYear { get; set; }
    public required int NumberOfDoors { get; set; }
    public required int NumberOfSeats { get; set; }
    public required decimal BootCapacity { get; set; }
    public required decimal Length { get; set; }
    public required decimal Height { get; set; }
    public required decimal Width { get; set; }
    public required decimal WheelBase { get; set; }
    public required  decimal BackWheelTrack { get; set; }
    public required decimal FrontWheelTrack { get; set; }
}