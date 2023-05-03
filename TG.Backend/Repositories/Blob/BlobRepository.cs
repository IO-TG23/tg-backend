namespace TG.Backend.Repositories.Blob;

public class BlobRepository : IBlobRepository
{
    private readonly AppDbContext _dbContext;

    public BlobRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateBlobAsync(Data.Blob blob)
    {
        await _dbContext.Blobs
            .AddAsync(blob);

        await _dbContext.SaveChangesAsync();

        return blob.Id;
    }

    public async Task<Data.Blob?> GetBlobByIdAsync(Guid id)
    {
        return await _dbContext.Blobs
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}