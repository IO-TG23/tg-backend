using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using TG.Backend.Data;
using TG.Backend.Repositories.Blob;

namespace TG.Backend.Tests.RepositoryTests;

public class BlobRepositoryTests
{
    private readonly Mock<AppDbContext> _contextMock = new();

    [Fact]
    public async Task GetBlobByIdAsync_ForNotExistingId_ReturnsNull()
    {
        // arrange
        _contextMock
            .Setup(x => x.Set<Blob>())
            .ReturnsDbSet(GetFakeBlobList());
        var repository = new BlobRepository(_contextMock.Object);

        // act
        var blob = await repository.GetBlobByIdAsync(Guid.NewGuid());

        // assert
        blob.Should().BeNull();
    }

    [Fact]
    public async Task GetBlobByIdAsync_ForExistingId_ReturnCorrectBlob()
    {
        // arrange
        var fakeBlobList = GetFakeBlobList().ToList();
        var guid = Guid.NewGuid();
        var newBlob = new Blob
        {
            Id = guid,
            Name = "Blob Unique",
            OfferId = Guid.Empty
        };
        fakeBlobList.Add(newBlob);
        _contextMock
            .Setup(x => x.Set<Blob>())
            .ReturnsDbSet(fakeBlobList);
        var repository = new BlobRepository(_contextMock.Object);

        // act
        var blob = await repository.GetBlobByIdAsync(guid);

        // assert
        blob.Should().NotBeNull();
        blob!.Name.Should().Be("Blob Unique");
    }

    [Fact]
    public async Task CreateBlobAsync_ForValidBlob_ShouldCallSaveChangesAsyncAndAddAsyncAndReturnCorrectBlobId()
    {
        // arrange
        _contextMock
            .Setup(x => x.Set<Blob>())
            .ReturnsDbSet(new List<Blob>());
        var repository = new BlobRepository(_contextMock.Object);
        var blob = new Blob
        {
            Name = "Name",
            OfferId = Guid.Empty
        };
        
        // act
        var id = await repository.CreateBlobAsync(blob);

        // assert
        _contextMock.Verify(x =>
            x.Set<Blob>().AddAsync(blob, default), Times.Once);
        _contextMock.Verify(x =>
            x.SaveChangesAsync(default), Times.Once);
        id.Should().Be(blob.Id);
    }

    private static IEnumerable<Blob> GetFakeBlobList()
    {
        return new List<Blob>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name1",
                OfferId = Guid.Empty
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name2",
                OfferId = Guid.Empty
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name3",
                OfferId = Guid.Empty
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "name4",
                OfferId = Guid.Empty
            }
        };
    }
}