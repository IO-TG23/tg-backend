using TG.Backend.Helpers;
using TG.Backend.Models.Offer;

namespace TG.Backend.Repositories.Offer;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _dbContext;

    public OfferRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Data.Offer>> GetOffersAsync(GetOffersFilterDTO filter)
    {
        var offers = await _dbContext.Offers
            .Include(o => o.Vehicle)
            .Where(o => filter.Gearbox == null || o.Vehicle.Gearbox == filter.Gearbox.GetGearbox())
            .Where(o => filter.Drive == null || o.Vehicle.Drive == filter.Drive.GetDrive())
            .Where(o => (filter.PriceLow == null || o.Price >= filter.PriceLow) && (filter.PriceHigh == null || o.Price <= filter.PriceHigh))
            .ToListAsync();

        return offers;
    }
    
    public async Task CreateOfferAsync(Data.Offer offer)
    {
        await _dbContext.Offers.AddAsync(offer);

        await _dbContext.SaveChangesAsync();
    }
}