using System.Data.Common;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;
using TG.Backend.Data;

namespace TG.Backend.Tests;

public class ApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient HttpClient { get; private set; } = default!;
    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptions = services.SingleOrDefault(service =>
                service.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (dbContextOptions is not null)
                services.Remove(dbContextOptions);

            var configuration = services.BuildServiceProvider().CreateScope().ServiceProvider
                .GetRequiredService<IConfiguration>();
            
            // replace default dbcontext with test context
            services.AddDbContext<AppDbContext>(op =>
                op.UseNpgsql(configuration.GetConnectionString("TestConn")));
                  
            // setup fake authentication and authorization
            services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
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
}