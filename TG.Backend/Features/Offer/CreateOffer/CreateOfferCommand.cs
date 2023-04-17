using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.CreateOffer;

public record CreateOfferCommand(CreateOfferDTO CreateOfferDto) : IRequest<OffersResponse>;