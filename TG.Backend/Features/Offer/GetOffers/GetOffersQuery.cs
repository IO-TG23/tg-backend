using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.GetOffers;

public record GetOffersQuery(GetOffersFilterDTO Filter) : IRequest<OffersResponse>;

    
