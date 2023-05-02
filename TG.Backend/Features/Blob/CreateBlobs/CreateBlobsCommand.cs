using TG.Backend.Models.Blob;

namespace TG.Backend.Features.Blob.CreateBlobs;

public record CreateBlobsCommand(IFormFileCollection Blobs, Guid OfferId) : IRequest<BlobResponse>;