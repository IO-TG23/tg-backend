namespace TG.Backend.Features.Vehicle.CreateVehicle;

public record CreateVehicleCommand(VehicleDTO VehicleDto) : IRequest<VehicleResponse>;