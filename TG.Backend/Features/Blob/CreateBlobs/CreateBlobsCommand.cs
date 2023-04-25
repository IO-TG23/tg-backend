using TG.Backend.Models.Blob;

namespace TG.Backend.Features.Blob.CreateBlobs;

public record CreateBlobsCommand(IFormFileCollection Blobs) : IRequest<BlobResponse>;