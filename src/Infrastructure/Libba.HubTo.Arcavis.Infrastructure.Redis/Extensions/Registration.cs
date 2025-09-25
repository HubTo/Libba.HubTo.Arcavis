using Libba.HubTo.Arcavis.Application.Interfaces.Caching;
using Libba.HubTo.Arcavis.Infrastructure.Redis.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Libba.HubTo.Arcavis.Infrastructure.Redis.Extensions;

public static class Registration
{
    public static IServiceCollection AddRedisRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis")
            ?? throw new ArgumentNullException("Redis connection string was not found.");

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
        services.AddScoped<ICacheService, RedisCacheService>();

        return services;
    }
}
