using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.EditOffer;

public record EditOfferCommand(EditOfferDTO EditOfferDto, Guid Id) : IRequest<OfferResponse>;