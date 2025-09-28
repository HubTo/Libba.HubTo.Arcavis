using Libba.HubTo.Arcavis.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using StackExchange.Redis;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
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
            // --- Adım 1: Gerçek PostgreSQL Kaydını Sök ---
            services.RemoveAll(typeof(DbContextOptions<ArcavisContext>));
            services.RemoveAll(typeof(ArcavisContext));

            // --- Adım 2: Gerçek Redis Kaydını Sök ---
            // Program.cs'in kaydettiği IConnectionMultiplexer'ı kaldırıyoruz.
            services.RemoveAll(typeof(IConnectionMultiplexer));

            // Not: ICacheService'i silmiyoruz, çünkü onun gerçek implementasyonunu kullanmak istiyoruz.

            // --- Adım 3: Test PostgreSQL Kaydını Tak ---
            services.AddDbContext<ArcavisContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });

            // --- Adım 4: Test Redis Kaydını Tak (Sihirli Kısım) ---
            // IConnectionMultiplexer'ı singleton olarak kaydediyoruz.
            // Ama onu hemen oluşturmuyoruz. DI konteyneri, ona ilk ihtiyaç duyulduğunda
            // aşağıdaki lambda fonksiyonunu çalıştıracak.
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                // Bu kod çalıştığı anda, InitializeAsync metodu çoktan bitmiş
                // ve _redisContainer başlamış olacak.
                var connectionString = _redisContainer.GetConnectionString() + ",abortConnect=false";
                return ConnectionMultiplexer.Connect(connectionString);
            });
        });
    }

    public async ValueTask InitializeAsync()
    {
        // Testler başlamadan ÖNCE container'ları başlat.
        await Task.WhenAll(
            _dbContainer.StartAsync(),
            _redisContainer.StartAsync()
        );

        // Veritabanı script'ini çalıştır.
        // ... (init.sql'i çalıştırma kodun burada) ...
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