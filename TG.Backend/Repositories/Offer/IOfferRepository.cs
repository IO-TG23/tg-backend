using TG.Backend.Models.Offer;

namespace TG.Backend.Repositories.Offer;

public interface IOfferRepository
{
    Task<IEnumerable<Data.Offer>> GetOffersAsync(GetOffersFilterDTO filter);
    Task CreateOfferAsync(Data.Offer vehicleDto);
    Task<Data.Offer?> GetOfferByIdAsync(Guid id, bool track = false);
    Task DeleteOfferAsync(Data.Offer offer);
    Task EditOfferAsync(Data.Offer currentOffer, EditOfferDTO editOfferDto);
    Task DeleteAllClientOffersAsync(DeleteAllClientOffersDTO clientDto);
}