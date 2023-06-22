using TG.Backend.Exceptions;
using TG.Backend.Models.Offer;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Features.Offer.GetOfferById;

public class GetOfferByIdQueryHandler : IRequestHandler<GetOfferByIdQuery, OfferResponse>
{
    private readonly IOfferRepository _offerRepository;

    public GetOfferByIdQueryHandler(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<OfferResponse> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await _offerRepository.GetOfferByIdAsync(request.Id);

        if (offer is null) throw new OfferNotFoundException(request.Id);

        return new OfferResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Offer = new DetailedOfferDTO
            {
                Price = offer.Price,
                BackWheelTrack = offer.Vehicle.BackWheelTrack,
                BootCapacity = offer.Vehicle.BootCapacity,
                ContactEmail = offer.ContactEmail,
                ContactPhoneNumber = offer.ContactPhoneNumber,
                VehicleName = offer.Vehicle.Name,
                VehicleDescription = offer.Vehicle.Description,
                ProductionStartYear = offer.Vehicle.ProductionStartYear,
                ProductionEndYear = offer.Vehicle.ProductionEndYear,
                FrontWheelTrack = offer.Vehicle.FrontWheelTrack,
                Height = offer.Vehicle.Height,
                Width = offer.Vehicle.Width,
                WheelBase = offer.Vehicle.WheelBase,
                Length = offer.Vehicle.Length,
                NumberOfDoors = offer.Vehicle.NumberOfDoors,
                NumberOfSeats = offer.Vehicle.NumberOfSeats,
                OfferDescription = offer.Description,
                BlobIds = offer.Blobs.Select(b => b.Id),
                ClientId = offer.Vehicle.ClientId
            }
        };
    }
}