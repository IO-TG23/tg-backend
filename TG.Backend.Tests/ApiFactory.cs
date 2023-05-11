using System.Data.Common;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Npgsql;
using Respawn;
using Respawn.Graph;
using TG.Backend.Data;
using TG.Backend.Models.Auth;

namespace TG.Backend.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    public UserManager<AppUser> UserManager { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptions = services.SingleOrDefault(service =>
                service.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextOptions is not null)
                services.Remove(dbContextOptions);
            var blobServiceClient = services.SingleOrDefault(service =>
                service.ServiceType == typeof(BlobServiceClient));
            if (blobServiceClient is not null)
                services.Remove(blobServiceClient);

            var configuration = services.BuildServiceProvider().CreateScope().ServiceProvider
                .GetRequiredService<IConfiguration>();

            // replace default dbcontext with test context
            services.AddDbContext<AppDbContext>(op =>
                op.UseNpgsql(configuration.GetConnectionString("TestConn"),
                    pgOpts => { pgOpts.SetPostgresVersion(new Version("9.6")); }));

            // replace default BlobServiceClient
            services.AddScoped(x => GetBlobServiceClientMock().Object);

            // setup fake authentication and authorization
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));

            UserManager = services.BuildServiceProvider().GetRequiredService<UserManager<AppUser>>();
        });

        base.ConfigureWebHost(builder);
    }

    public async Task ResetDatabaseAsync()
    {
        // clean tables in database
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        var configuration = Services.CreateScope().ServiceProvider
            .GetRequiredService<IConfiguration>();

        _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("TestConn"));

        await InitializeRespawnerAsync();
        HttpClient = CreateClient();
    }

    private async Task InitializeRespawnerAsync()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            SchemasToInclude = new[] { "public" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
    }

    private static Mock<BlobServiceClient> GetBlobServiceClientMock()
    {
        var blobContainerClientMock = new Mock<BlobContainerClient>();
        var blobServiceClientMock = new Mock<BlobServiceClient>();
        var blobClientMock = new Mock<BlobClient>();
        var blobResponseDownloadResultMock = new Mock<Response<BlobDownloadResult>>();

        var blobDownloadResult = BlobsModelFactory.BlobDownloadResult(details: BlobsModelFactory.BlobDownloadDetails(
            blobType: BlobType.Block,
            contentLength: 1024,
            contentType: "application/octet-stream",
            contentHash: new byte[] { 0x01, 0x23, 0x45, 0x67 },
            lastModified: DateTimeOffset.Now,
            metadata: new Dictionary<string, string>() { { "key1", "value1" }, { "key2", "value2" } },
            contentRange: "bytes 0-1023/2048",
            contentEncoding: "gzip",
            cacheControl: "max-age=3600",
            contentDisposition: "attachment; filename=\"example.txt\"",
            contentLanguage: "en-US",
            blobSequenceNumber: 1,
            copyCompletedOn: DateTimeOffset.Now.AddDays(-1),
            copyStatusDescription: "Success",
            copyId: "01234567-89ab-cdef-0123-456789abcdef",
            copyProgress: "100%",
            copySource: new Uri("https://example.blob.core.windows.net/source-container/source-blob"),
            copyStatus: CopyStatus.Success,
            leaseDuration: LeaseDurationType.Fixed,
            leaseState: LeaseState.Leased,
            leaseStatus: LeaseStatus.Locked,
            acceptRanges: "bytes",
            blobCommittedBlockCount: 4,
            isServerEncrypted: true,
            encryptionKeySha256: "0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcdef",
            encryptionScope: "myEncryptionScope",
            blobContentHash: new byte[] { 0x89, 0xab, 0xcd, 0xef },
            tagCount: 2,
            versionId: "01234567-89ab-cdef-0123-456789abcdef",
            isSealed: true,
            objectReplicationSourceProperties: new List<ObjectReplicationPolicy>(),
            objectReplicationDestinationPolicy: "object-replication-policy",
            hasLegalHold: true,
            createdOn: DateTimeOffset.Now.AddDays(-7)));

        var file = File.OpenRead("../../../2cde3011-2f06-485d-86ac-30575af5519c");

        blobResponseDownloadResultMock
            .Setup(b => b.Value)
            .Returns(blobDownloadResult);

        blobClientMock
            .Setup(b => b.OpenReadAsync(It.IsAny<long>(), It.IsAny<int?>(), It.IsAny<BlobRequestConditions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((() => file));

        blobClientMock
            .Setup(b => b.DownloadContentAsync())
            .ReturnsAsync(blobResponseDownloadResultMock.Object);

        blobContainerClientMock
            .Setup(b => b.GetBlobClient(It.IsAny<string>()))
            .Returns(blobClientMock.Object);

        blobServiceClientMock
            .Setup(b => b.GetBlobContainerClient(It.IsAny<string>()))
            .Returns(blobContainerClientMock.Object);

        return blobServiceClientMock;
    }
}