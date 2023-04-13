namespace TG.Backend.Repositories.Vehicle;

public interface IVehicleRepository
{
    Task<IEnumerable<Data.Vehicle>> GetVehiclesAsync();
    Task CreateVehicleAsync(Data.Vehicle vehicleDto);
}