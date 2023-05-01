using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TG.Backend.Data;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace TG.Backend.Tests.OfferControllerTests;

public class OfferControllerTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public OfferControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((_, config) =>
                { 
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("testSettings.json")
                        .Build();
 
                    config.AddConfiguration(configuration);
                });
                
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (dbContextOptions is not null)
                        services.Remove(dbContextOptions);

                    var configuration = services.BuildServiceProvider().CreateScope().ServiceProvider
                        .GetRequiredService<IConfiguration>();
                    
                    services.AddDbContext<AppDbContext>(op =>
                        op.UseNpgsql(configuration.GetConnectionString("TestConn")));
                });
            });
        _client = _factory.CreateClient();
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
        var offer = new Offer
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
    
    public void Dispose()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureDeleted();
        _client.Dispose();
        _factory.Dispose();
    }

    private async Task Seed(Offer offer)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Offers.AddAsync(offer);
        await context.SaveChangesAsync();
    }
}