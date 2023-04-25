using TG.Backend.Models.Blob;

namespace TG.Backend.Services;

public interface IAzureBlobStorageService
{
    Task<BlobDTO> GetBlobDtoByIdAsync(Guid blobId);
    Task UploadBlobAsync(Guid blobId, IFormFile blob);
}