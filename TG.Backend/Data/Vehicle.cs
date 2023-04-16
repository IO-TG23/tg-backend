namespace TG.Backend.Data;

public sealed class Vehicle : Entity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
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
    public required Gearbox Gearbox { get; set; }
    public required Drive Drive { get; set; }
    public Offer Offer { get; set; } = null!;
}