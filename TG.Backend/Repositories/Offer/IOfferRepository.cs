namespace TG.Backend.Repositories.Offer;

public interface IOfferRepository
{
    Task<IEnumerable<Data.Offer>> GetOffersAsync();
    Task CreateOfferAsync(Data.Offer vehicleDto);
}