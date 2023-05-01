using System.Net;
using TG.Backend.Data;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TG.Backend.Models.Offer;
using TG.Backend.Models.Vehicle;

namespace TG.Backend.Tests.OfferControllerTests;

[Collection("SharedApi")]
public class OfferControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly ApiFactory _factory;
    
    public OfferControllerTests(ApiFactory apiFactory)
    {
        _factory = apiFactory;
        _client = apiFactory.HttpClient;
        _resetDatabase = apiFactory.ResetDatabaseAsync;
    }

    [Theory]
    [InlineData("drive=amg&gearbox=pwd&priceHigh=20&priceLow=10")]
    [InlineData("drive=fwd&gearbox=fdf&priceHigh=20&priceLow=10")]
    [InlineData("drive=kgm&gearbox=manual&priceHigh=20&priceLow=10")]
    public async Task GetOffers_ForInvalidFilters_ReturnsBadRequestResult(string queryParams)
    {
        // act
        var response = await _client.GetAsync($"/Offer?{queryParams}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("drive=fwd&gearbox=automatic&priceHigh=20&priceLow=10")]
    [InlineData("drive=rwd&gearbox=manual&priceHigh=20&priceLow=10")]
    [InlineData("drive=fwd&gearbox=manual&priceHigh=20&priceLow=10")]
    public async Task GetOffers_ForValidFilters_ReturnsOkResult(string queryParams)
    {
        // act
        var response = await _client.GetAsync($"/Offer?{queryParams}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetOfferBydId_ForValidId_ReturnsOkResult()
    {
        // arrange
        var offer = GetValidOffer();
        await Seed(offer);

        // act
        var response = await _client.GetAsync($"Offer/{offer.Id}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
    }

    [Fact]
    public async Task GetOfferBydId_ForInvalidId_ReturnsNotFoundResult()
    {
        // act
        var response = await _client.GetAsync($"Offer/{Guid.Empty}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateOffer_ForValidModel_ReturnsNoContentResult()
    {
        // arrange
        var offer = new CreateOfferDTO
        {
            Vehicle = new VehicleDTO
            {
                Name = "name",
                Description = "desc",
                ProductionStartYear = 1950,
                ProductionEndYear = 2000,
                NumberOfDoors = 1,
                NumberOfSeats = 1,
                BootCapacity = 1,
                Length = 1,
                Height = 1,
                Width = 1,
                WheelBase = 1,
                BackWheelTrack = 1,
                FrontWheelTrack = 1,
                Gearbox = "Automatic",
                Drive = "AWD"
            },
            Price = 1500,
            Description = "desc",
            ContactEmail = "test@test.com",
            ContactPhoneNumber = "123456789"
        };
        var content = offer.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("Offer", content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task CreateOffer_ForInvalidModel_ReturnsBadRequestResult()
    {
        // arrange
        var offer = new CreateOfferDTO
        {
            Vehicle = new VehicleDTO
            {
                Name = null,
                Description = null,
                ProductionStartYear = 0,
                ProductionEndYear = null,
                NumberOfDoors = 0,
                NumberOfSeats = 0,
                BootCapacity = 0,
                Length = 0,
                Height = 0,
                Width = 0,
                WheelBase = 0,
                BackWheelTrack = 0,
                FrontWheelTrack = 0,
                Gearbox = null,
                Drive = null
            },
            Price = 1500,
            Description = "desc",
            ContactEmail = "test@test.com",
            ContactPhoneNumber = "123456789"
        };
        var content = offer.ToJsonHttpContent();

        // act
        var response = await _client.PostAsync("Offer", content);

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task DeleteOfferById_ForValidId_ReturnsNoContentResult()
    {
        // arrange
        var offer = GetValidOffer();
        await Seed(offer);

        // act
        var response = await _client.DeleteAsync($"Offer/{offer.Id}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteOfferBydId_ForInvalidId_ReturnsNotFoundResult()
    {
        // act
        var response = await _client.DeleteAsync($"Offer/{Guid.Empty}");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    private async Task Seed(Offer offer)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Offers.AddAsync(offer);
        await context.SaveChangesAsync();
    }

    private Offer GetValidOffer()
    {
        return new Offer
        {
            Price = 50,
            Description = "desc",
            ContactEmail = "test@test.com",
            ContactPhoneNumber = "123456789",
            Vehicle = new Vehicle
            {
                Name = "name",
                Description = "desc",
                ProductionStartYear = 1950,
                ProductionEndYear = 2000,
                NumberOfDoors = 1,
                NumberOfSeats = 1,
                BootCapacity = 1,
                Length = 1,
                Height = 1,
                Width = 1,
                WheelBase = 1,
                BackWheelTrack = 1,
                FrontWheelTrack = 1,
                Gearbox = Gearbox.Automatic,
                Drive = Drive.AWD,
            }
        };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    // clean tables after each test
    public async Task DisposeAsync() => await _resetDatabase();
}