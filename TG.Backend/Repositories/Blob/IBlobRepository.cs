namespace TG.Backend.Repositories.Blob;

public interface IBlobRepository
{
    Task<Guid> CreateBlobAsync(Data.Blob blob);
    Task<Data.Blob?> GetBlobByIdAsync(Guid id);
}