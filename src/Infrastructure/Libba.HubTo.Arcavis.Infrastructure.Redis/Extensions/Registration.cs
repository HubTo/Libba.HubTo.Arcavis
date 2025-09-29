using Libba.HubTo.Arcavis.Application.Interfaces.Caching;
using Libba.HubTo.Arcavis.Infrastructure.Redis.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Polly;

namespace Libba.HubTo.Arcavis.Infrastructure.Redis.Extensions;

public static class Registration
{
    public static IServiceCollection AddRedisRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis")
            ?? throw new ArgumentNullException(nameof(configuration), "Redis connection string was not found.");

        var retryPolicy = Policy
            .Handle<RedisConnectionException>()
            .WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4)
            });

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            return retryPolicy.Execute(() => ConnectionMultiplexer.Connect(connectionString));
        });

        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
