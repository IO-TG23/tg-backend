namespace TG.Backend.Models.Offer;

public class OffersResponse : ResponseModel
{
    public IEnumerable<GetOfferDTO>? Offers { get; set; }
}