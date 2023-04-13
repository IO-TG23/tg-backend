namespace TG.Backend.Repositories.Vehicle;

public class VehicleRepository : IVehicleRepository
{
    private readonly AppDbContext _dbContext;

    public VehicleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Data.Vehicle>> GetVehiclesAsync()
    {
        var vehicles = await _dbContext.Vehicles
            .ToListAsync();

        return vehicles;
    }

    public async Task CreateVehicleAsync(Data.Vehicle vehicle)
    {
        await _dbContext.AddAsync(vehicle);

        await _dbContext.SaveChangesAsync();
    }
}