using TG.Backend.Models.Blob;

namespace TG.Backend.Features.Blob.GetBlobById;

public record GetBlobByIdQuery(Guid Id): IRequest<BlobResponse>;