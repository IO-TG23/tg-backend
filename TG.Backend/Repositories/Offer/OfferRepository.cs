using AutoMapper;
using TG.Backend.Helpers;
using TG.Backend.Models.Offer;

namespace TG.Backend.Repositories.Offer;

public class OfferRepository : IOfferRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public OfferRepository(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Data.Offer>> GetOffersAsync(GetOffersFilterDTO filter)
    {
        var offers = await _dbContext.Offers
            .AsNoTracking()
            .Include(o => o.Vehicle)
            .Include(o => o.Blobs)
            .Where(o => filter.Gearbox == null || o.Vehicle.Gearbox == filter.Gearbox.GetGearbox())
            .Where(o => filter.Drive == null || o.Vehicle.Drive == filter.Drive.GetDrive())
            .Where(o => (filter.PriceLow == null || o.Price >= filter.PriceLow) &&
                        (filter.PriceHigh == null || o.Price <= filter.PriceHigh))
            .ToListAsync();

        return offers;
    }

    public async Task CreateOfferAsync(Data.Offer offer)
    {
        await _dbContext.Offers.AddAsync(offer);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Data.Offer?> GetOfferByIdAsync(Guid id, bool track = false)
    {
        if (!track)
            return await _dbContext.Offers
                .AsNoTracking()
                .Include(o => o.Vehicle)
                .Include(o => o.Blobs)
                .FirstOrDefaultAsync(o => o.Id == id);

        return await _dbContext.Offers
            .Include(o => o.Vehicle)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task DeleteOfferAsync(Data.Offer offer)
    {
        _dbContext.Offers.Remove(offer);

        await _dbContext.SaveChangesAsync();
    }

    public async Task EditOfferAsync(Data.Offer currentOffer, EditOfferDTO editOfferDto)
    {
        currentOffer.ContactEmail = editOfferDto.OfferDto.ContactEmail;
        currentOffer.ContactPhoneNumber = editOfferDto.OfferDto.ContactPhoneNumber;
        currentOffer.Description = editOfferDto.OfferDto.Description;
        currentOffer.Price = editOfferDto.OfferDto.Price;
        currentOffer.Vehicle = _mapper.Map<Data.Vehicle>(editOfferDto.OfferDto.Vehicle);

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAllClientOffersAsync(DeleteAllClientOffersDTO clientDto)
    {
        Guid? clientId = await _dbContext.Clients
            .Include(c => c.AppUser)
            .Where(c => c.AppUser.Email == clientDto.Email)
            .Select(c => c.Id)
            .FirstOrDefaultAsync();

        if (clientId == Guid.Empty)
        {
            return;
        }

        List<Vehicle> vehicles = await _dbContext.Vehicles.Include(v => v.Client)
            .Where(v => v.ClientId == clientId).ToListAsync();

        _dbContext.Vehicles.RemoveRange(vehicles);

        await _dbContext.SaveChangesAsync();
    }
}