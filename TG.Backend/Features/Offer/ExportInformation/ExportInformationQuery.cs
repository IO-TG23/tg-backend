using TG.Backend.Models.Offer;

namespace TG.Backend.Features.Offer.ExportInformation;

public record ExportInformationQuery(Guid Id, string Email) : IRequest<OfferResponse>;
