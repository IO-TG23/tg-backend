using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using TG.Backend.Models.Blob;

namespace TG.Backend.Services;

public class AzureBlobStorageService : IAzureBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;

    public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public async Task<BlobDTO> GetBlobDtoByIdAsync(Guid blobId)
    {
        var containerClient = await GetContainerClientAsync();
        var blobClient = containerClient.GetBlobClient(blobId.ToString());
        var data = await blobClient.OpenReadAsync();

        var downloadResult = await blobClient.DownloadContentAsync();
        var contentType = downloadResult.Value.Details.ContentType;

        return new BlobDTO { Data = data, ContentType = contentType };
    }

    public async Task UploadBlobAsync(Guid blobId, IFormFile blob)
    {
        var containerClient = await GetContainerClientAsync();
        var blobClient = containerClient.GetBlobClient(blobId.ToString());

        await using var data = blob.OpenReadStream();

        await blobClient.UploadAsync(data, new BlobHttpHeaders
        {
            ContentType = blob.ContentType
        });
    }

    private async Task<BlobContainerClient> GetContainerClientAsync()
    {
        var blobContainerClient =
            _blobServiceClient.GetBlobContainerClient(_configuration["ConnectionStrings:AzureBlobContainerName"]);

        await blobContainerClient.CreateIfNotExistsAsync();

        return blobContainerClient;
    }
}