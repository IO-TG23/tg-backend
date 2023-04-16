using TG.Backend.Models.Offer;

namespace TG.Backend.Repositories.Offer;

public interface IOfferRepository
{
    Task<IEnumerable<Data.Offer>> GetOffersAsync(GetOffersFilterDTO filter);
    Task CreateOfferAsync(Data.Offer vehicleDto);
}