using TG.Backend.Models.Offer;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Features.Offer.DeleteAllClientOffers
{
    public class DeleteAllClientOffersHandler : IRequestHandler<DeleteAllClientOffersCommand, OfferResponse>
    {
        private readonly IOfferRepository _offerRepository;

        public DeleteAllClientOffersHandler(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }

        public async Task<OfferResponse> Handle(DeleteAllClientOffersCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _offerRepository.DeleteAllClientOffersAsync(request.Client);

                return new() { IsSuccess = true, Messages = Array.Empty<string>(), StatusCode = HttpStatusCode.NoContent };
            }
            catch (Exception ex)
            {
                return new() { IsSuccess = false, Messages = new[] { ex.Message }, StatusCode = HttpStatusCode.ServiceUnavailable };
            }
        }
    }
}
