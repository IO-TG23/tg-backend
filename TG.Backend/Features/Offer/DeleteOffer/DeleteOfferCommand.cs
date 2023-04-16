using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.DeleteOffer;

public record DeleteOfferCommand(Guid Id) : IRequest<OfferResponse>;