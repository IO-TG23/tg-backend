using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.DeleteAllClientOffers
{
    public record DeleteAllClientOffersCommand(DeleteAllClientOffersDTO Client) : IRequest<OfferResponse>;
}
