namespace TG.Backend.Models.Vehicle;

public class VehicleResponse : ResponseModel
{
    public IEnumerable<VehicleDTO>? Vehicles { get; set; }
}