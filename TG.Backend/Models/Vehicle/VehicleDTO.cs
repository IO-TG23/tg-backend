namespace TG.Backend.Models.Vehicle;

public class VehicleDTO
{
    public required string Name { get; set; }
    public required int ProductionStartYear { get; set; }
    public int? ProductionEndYear { get; set; }
    public required int NumberOfDoors { get; set; }
    public required int NumberOfSeats { get; set; }
    public required decimal BootCapacity { get; set; }
    public required decimal Length { get; set; }
    public required decimal Height { get; set; }
    public required decimal Width { get; set; }
    public required decimal WheelBase { get; set; }
    public required decimal BackWheelTrack { get; set; }
    public required decimal FrontWheelTrack { get; set; }
    public required string Gearbox { get; set; }
    public required string Drive { get; set; }
}