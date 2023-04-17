using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.GetOfferById;

public record GetOfferByIdQuery(Guid Id) : IRequest<OfferResponse>;
