using Microsoft.AspNetCore.Mvc;
using TG.Backend.Exceptions;
using TG.Backend.Models.Blob;
using TG.Backend.Repositories.Blob;
using TG.Backend.Services;

namespace TG.Backend.Features.Blob.GetBlobById;

public class GetBlobByIdQueryHandler : IRequestHandler<GetBlobByIdQuery, BlobResponse>
{
    private readonly IBlobRepository _blobRepository;
    private readonly IAzureBlobStorageService _azureBlobStorageService;

    public GetBlobByIdQueryHandler(IBlobRepository blobRepository, IAzureBlobStorageService azureBlobStorageService)
    {
        _blobRepository = blobRepository;
        _azureBlobStorageService = azureBlobStorageService;
    }

    public async Task<BlobResponse> Handle(GetBlobByIdQuery request, CancellationToken cancellationToken)
    {
        var blob = await _blobRepository.GetBlobByIdAsync(request.Id);
        if (blob is null)
            throw new BlobNotFoundException(request.Id);

        var blobDto = await _azureBlobStorageService.GetBlobDtoByIdAsync(request.Id);
        using var memoryStream = new MemoryStream();
        await blobDto.Data.CopyToAsync(memoryStream, cancellationToken);
        var data = memoryStream.ToArray();
        await memoryStream.DisposeAsync();

        return new BlobResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Blob = new FileContentResult(data, blobDto.ContentType)
            {
                FileDownloadName = blob.Name
            }
        };
    }
}