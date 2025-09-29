using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using StackExchange.Redis;
using Npgsql;
using Xunit;

public class ArcavisApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer;
    private readonly RedisContainer _redisContainer;

    public ArcavisApiFactory()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .WithDatabase("arcavis_test_db")
            .WithUsername("testuser")
            .WithPassword("testpass")
            .Build();

        _redisContainer = new RedisBuilder()
            .WithImage("redis:7-alpine")
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<ArcavisContext>));
            services.RemoveAll(typeof(ArcavisContext));

            services.RemoveAll(typeof(IConnectionMultiplexer));

            services.AddDbContext<ArcavisContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connectionString = _redisContainer.GetConnectionString() + ",abortConnect=false";
                return ConnectionMultiplexer.Connect(connectionString);
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        await Task.WhenAll(
            _dbContainer.StartAsync(),
            _redisContainer.StartAsync()
        );

        var initScriptPath = Path.Combine(AppContext.BaseDirectory, "DbScript/init.sql");
        if (!File.Exists(initScriptPath))
        {
            throw new FileNotFoundException("Database script not found!", initScriptPath);
        }
        var initScript = await File.ReadAllTextAsync(initScriptPath);
        await using var connection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(initScript, connection);
        await command.ExecuteNonQueryAsync();

    }

    public async Task DisposeAsync()
    {
        await Task.WhenAll(
            _dbContainer.DisposeAsync().AsTask(),
            _redisContainer.DisposeAsync().AsTask()
        );
    }
}