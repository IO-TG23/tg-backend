using AutoMapper;
using FluentAssertions;
using Moq;
using Moq.EntityFrameworkCore;
using TG.Backend.Data;
using TG.Backend.Helpers;
using TG.Backend.Models.Offer;
using TG.Backend.Models.Vehicle;
using TG.Backend.Repositories.Offer;

namespace TG.Backend.Tests.RepositoryTests;

public class OfferRepositoryTests
{
    private readonly Mock<AppDbContext> _contextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    [Fact]
    public async Task GetOffers_ForEmptyFilter_ReturnsAllOffers()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);

        // act
        var offers = await repository.GetOffersAsync(new GetOffersFilterDTO());

        // assert
        offers.Count().Should().Be(4);
    }

    [Fact]
    public async Task GetOffers_ForNotEmptyFilter_ReturnCorrectOffers()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);
        var filter = new GetOffersFilterDTO
        {
            Drive = "FWD",
            Gearbox = "Automatic",
            PriceLow = 500,
            PriceHigh = 1500
        };

        // act
        var offers = (await repository.GetOffersAsync(filter)).ToList();

        // assert
        offers.Count.Should().Be(1);
        offers.All(o =>
                o.Price >= filter.PriceLow && 
                o.Price <= filter.PriceHigh &&
                o.Vehicle.Gearbox == filter.Gearbox.GetGearbox() && 
                o.Vehicle.Drive == filter.Drive.GetDrive())
            .Should().BeTrue();
    }

    [Fact]
    public async Task GetOfferById_ForValidId_ReturnsCorrectOffer()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);

        // act
        var offer = await repository.GetOfferByIdAsync(Guid.Empty);

        // assert
        offer.Should().NotBeNull();
        offer.Description.Should().Be("offer");
    }

    [Fact]
    public async Task GetOfferById_ForNotExistingId_ReturnsNull()
    {
        
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);

        // act
        var offer = await repository.GetOfferByIdAsync(Guid.NewGuid());

        // assert
        offer.Should().BeNull();
    }

    [Fact]
    public async Task CreateOffer_ForValidOffer_ShouldCallSaveChangesAsyncOnContextAndAddAsyncOnDbSet()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);
        var addedObject = GetFakeOfferList().First();

        // act
        await repository.CreateOfferAsync(addedObject);

        // assert
        _contextMock.Verify(x => 
            x.Set<Offer>().AddAsync(addedObject, default), Times.Once);
        _contextMock.Verify(x => 
            x.SaveChangesAsync(default), Times.Once);
    }
    
    [Fact]
    public async Task DeleteOffer_ForValidOffer_ShouldCallSaveChangesAsyncOnContextAndRemoveOnDbSet()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);
        var removedObject = GetFakeOfferList().First();

        // act
        await repository.DeleteOfferAsync(removedObject);

        // assert
        _contextMock.Verify(x => 
            x.Set<Offer>().Remove(removedObject), Times.Once);
        _contextMock.Verify(x => 
            x.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task EditOffer_ForValidModels_ShouldCallSaveChangesAsyncOnDbContextAndModifyCurrentOffer()
    {
        // arrange
        _contextMock.Setup(x => x.Set<Offer>())
            .ReturnsDbSet(GetFakeOfferList());
        var repository = new OfferRepository(_contextMock.Object, _mapperMock.Object);
        var editedOffer = GetFakeOfferList().First();
        var editOfferDto = new EditOfferDTO
        {
            OfferDto = new CreateOfferDTO
            {
                Vehicle = new VehicleDTO
                {
                    Name = "name1",
                    Description = "desc1",
                    ProductionStartYear = 1960,
                    ProductionEndYear = 2010,
                    NumberOfDoors = 2,
                    NumberOfSeats = 2,
                    BootCapacity = 2,
                    Length = 2,
                    Height = 2,
                    Width = 2,
                    WheelBase = 2,
                    BackWheelTrack = 2,
                    FrontWheelTrack = 2,
                    Gearbox = "Manual",
                    Drive = "FWD",
                    ClientId = default
                },
                Price = 1950,
                Description = "desc1",
                ContactEmail = "test1@test.com",
                ContactPhoneNumber = "987654321"
            }
        };

        // act
        await repository.EditOfferAsync(editedOffer, editOfferDto);

        // assert
        _contextMock.Verify(x => 
            x.SaveChangesAsync(default), Times.Once);
        editedOffer.Description.Should().Be("desc1");
        editedOffer.Price.Should().Be(1950);
        editedOffer.ContactPhoneNumber.Should().Be("987654321");
    }

    private static List<Offer> GetFakeOfferList()
    {
        return new List<Offer>()
        {
            new()
            {
                Id = Guid.NewGuid(),
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
                    Drive = Drive.FWD
                },
                VehicleId = Guid.Empty
            },
            new()
            {
                Id = Guid.Empty,
                Price = 1700,
                Description = "offer",
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
                    Drive = Drive.FWD
                },
                VehicleId = Guid.Empty
            },
            new()
            {
                Id = Guid.NewGuid(),
                Price = 400,
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
                    Drive = Drive.FWD
                },
                VehicleId = Guid.Empty
            },
            new()
            {
                Id = Guid.NewGuid(),
                Price = 1200,
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
                    Gearbox = Gearbox.Manual,
                    Drive = Drive.AWD
                },
                VehicleId = Guid.Empty
            }
        };
    }
}