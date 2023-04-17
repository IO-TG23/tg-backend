using AutoMapper;
using TG.Backend.Models.Offer;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Features.Offer.GetOffers;

public class GetOffersQueryHandler : IRequestHandler<GetOffersQuery, OffersResponse>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public GetOffersQueryHandler(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<OffersResponse> Handle(GetOffersQuery request, CancellationToken cancellationToken)
    {
        var offers = (await _offerRepository.GetOffersAsync(request.Filter))
            .Select(o => new GetOfferDTO
            {
                Id = o.Id,
                VehicleDescription = o.Vehicle.Description,
                VehicleName = o.Vehicle.Name
            });

        return new OffersResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Offers = _mapper.Map<IEnumerable<GetOfferDTO>>(offers)
        };
    }
}