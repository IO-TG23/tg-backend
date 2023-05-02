using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using TG.Backend.Data;
using TG.Backend.Repositories.Offer;
using Xunit.Abstractions;

namespace TG.Backend.Tests.RepositoryTests;

public class OfferRepositoryTests
{
    private readonly Mock<AppDbContext> _contextMock = new();
    
    [Fact]
    public async Task GetOfferById_ForValidId_ReturnsCorrectOffer()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object);

        // act
        var offer = await repository.GetOfferByIdAsync(Guid.Empty);

        // assert
        offer?.Should().NotBeNull();
        offer?.Description.Should().Be("desc");
    }

    [Fact]
    public async Task CreateOffer_ForValidModel_ShouldCallSaveChangesAsyncOnDbContext()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(new List<Offer>());
        var repository = new OfferRepository(_contextMock.Object);

        // act
        await repository.CreateOfferAsync(GetFakeOfferList().First());
        
        // assert
        _contextMock.Verify(
            x => x.SaveChangesAsync(default), Times.Once);
    }
    
    private static List<Offer> GetFakeOfferList()
    {
        return new List<Offer>()
        {
            new Offer
            {
                Id = Guid.Empty,
                Price = 1500,
                Description = "desc",
                ContactEmail = "test@test.com",
                ContactPhoneNumber = "123456789",
                Vehicle = new Vehicle
                {
                    Id = Guid.Empty,
                    Name = "name",
                    Description = "desc",
                    ProductionStartYear = 1950,
                    NumberOfDoors = 2000,
                    NumberOfSeats = 1,
                    BootCapacity = 1,
                    Length = 1,
                    Height = 1,
                    Width = 1,
                    WheelBase = 1,
                    BackWheelTrack = 1,
                    FrontWheelTrack = 1,
                    Gearbox = Gearbox.Automatic,
                    Drive = Drive.AWD
                },
                VehicleId = Guid.Empty
            }
        };
    }
}