namespace TG.Backend.Repositories.Offer;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _dbContext;

    public OfferRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Data.Offer>> GetOffersAsync()
    {
        var offers = await _dbContext.Offers
            .Include(o => o.Vehicle)
            .ToListAsync();

        return offers;
    }
    
    public async Task CreateOfferAsync(Data.Offer offer)
    {
        await _dbContext.Offers.AddAsync(offer);

        await _dbContext.SaveChangesAsync();
    }
}