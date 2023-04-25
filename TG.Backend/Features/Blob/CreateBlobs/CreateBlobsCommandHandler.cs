using TG.Backend.Models.Blob;
using TG.Backend.Repositories.Blob;
using TG.Backend.Services;

namespace TG.Backend.Features.Blob.CreateBlobs;

public class CreateBlobsCommandHandler : IRequestHandler<CreateBlobsCommand, BlobResponse>
{
    private readonly IBlobRepository _blobRepository;
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public CreateBlobsCommandHandler(IBlobRepository blobRepository, IAzureBlobStorageService azureBlobStorageService)
    {
        _blobRepository = blobRepository;
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<BlobResponse> Handle(CreateBlobsCommand request, CancellationToken cancellationToken)
    {
        foreach (var blobFile in request.Blobs)
        {
            var blob = new Data.Blob
            {
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