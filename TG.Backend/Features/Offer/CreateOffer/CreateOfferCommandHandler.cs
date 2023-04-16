using AutoMapper;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Features.Offer.CreateOffer;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferResponse>
{
    private readonly IOfferRepository _offerRepository;
    private readonly IMapper _mapper;

    public CreateOfferCommandHandler(IOfferRepository offerRepository, IMapper mapper)
    {
        _offerRepository = offerRepository;
        _mapper = mapper;
    }

    public async Task<OfferResponse> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var offer = new Data.Offer
        {
            ContactEmail = request.CreateOfferDto.ContactEmail,
            ContactPhoneNumber = request.CreateOfferDto.ContactPhoneNumber,
            Description = request.CreateOfferDto.Description,
            Price = request.CreateOfferDto.Price,
            Vehicle = _mapper.Map<Data.Vehicle>(request.CreateOfferDto.Vehicle)
        };

        await _offerRepository.CreateOfferAsync(offer);

        return new OfferResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent
        };
    }
}