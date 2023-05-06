using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using TG.Backend.Data;

namespace TG.Backend.Tests.ControllerTests;

[Collection("SharedApi")]
public class BlobControllerTests : IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly Func<Task> _resetDatabase;
    private readonly ApiFactory _factory;

    public BlobControllerTests(ApiFactory factory)
    {
        _factory = factory;
        _client = factory.HttpClient;
        _resetDatabase = factory.ResetDatabaseAsync;
    }

    [Fact]
    public async Task GetBlob_ForInvalidId_ReturnsNotFoundResult()
    {
        // act
        var response = await _client.GetAsync($"Blob/{Guid.NewGuid()}");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetBlob_ForValidId_ReturnsOkResult()
    {
        // arrange
        var offer = OfferControllerTests.GetValidOffer();
        await SeedOffer(offer);
        var blob = new Blob
        {
            Name = "Name",
            OfferId = offer.Id
        };
        await SeedBlob(blob);
        // act
        var response = await _client.GetAsync($"Blob/{blob.Id}");
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateBlobs_ForInvalidOfferId_ReturnsNotFoundResult()
    {
        // act
        var response = await _client.PostAsync($"Blob/{Guid.NewGuid()}", null);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateBlobs_ForValidOfferId_ReturnsNoContentResult()
    {
        // arrange
        var offer = OfferControllerTests.GetValidOffer();
        await SeedOffer(offer);
        var formData = new MultipartFormDataContent();
        var file = await File.ReadAllBytesAsync("../../../2cde3011-2f06-485d-86ac-30575af5519c");
        var fileContent = new ByteArrayContent(file);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        formData.Add(fileContent, "blobs", "2cde3011-2f06-485d-86ac-30575af5519c");
        
        // act
        var response = await _client.PostAsync($"Blob/{offer.Id}", formData);
        
        // assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    private async Task SeedOffer(Offer offer)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Offers.AddAsync(offer);
        await context.SaveChangesAsync();
    }
    
    private async Task SeedBlob(Blob blob)
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Blobs.AddAsync(blob);
        await context.SaveChangesAsync();
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync() => await _resetDatabase();
}