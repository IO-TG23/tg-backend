using TG.Backend.Models.Offer;

namespace TG.Backend.Models.Vehicle;

public class OfferResponse : ResponseModel
{
    public IEnumerable<GetOfferDTO>? Offers { get; set; }
}