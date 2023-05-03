using TG.Backend.Exceptions;
using TG.Backend.Models.Blob;
using TG.Backend.Repositories.Blob;
using TG.Backend.Repositories.Offer;
using TG.Backend.Services;

namespace TG.Backend.Features.Blob.CreateBlobs;

public class CreateBlobsCommandHandler : IRequestHandler<CreateBlobsCommand, BlobResponse>
{
    private readonly IBlobRepository _blobRepository;
    private readonly IAzureBlobStorageService _azureBlobStorageService;
    private readonly IOfferRepository _offerRepository;
    
    public CreateBlobsCommandHandler(IBlobRepository blobRepository, IAzureBlobStorageService azureBlobStorageService, IOfferRepository offerRepository)
    {
        _blobRepository = blobRepository;
        _azureBlobStorageService = azureBlobStorageService;
        _offerRepository = offerRepository;
    }

    public async Task<BlobResponse> Handle(CreateBlobsCommand request, CancellationToken cancellationToken)
    {
        if (await _offerRepository.GetOfferByIdAsync(request.OfferId) is null)
            throw new OfferNotFoundException(request.OfferId);
        
        foreach (var blobFile in request.Blobs)
        {
            var blob = new Data.Blob
            {
                OfferId = request.OfferId,
                Name = blobFile.FileName
            };

            var blobId = await _blobRepository.CreateBlobAsync(blob);
            await _azureBlobStorageService.UploadBlobAsync(blobId, blobFile);
        }
        
        return new BlobResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.NoContent
        };
    }
}