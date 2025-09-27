using Libba.HubTo.Arcavis.Application.Interfaces.Caching;
using StackExchange.Redis;
using System.Text.Json;

namespace Libba.HubTo.Arcavis.Infrastructure.Redis.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        var redisValue = await _database.StringGetAsync(key);
        if (redisValue.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<T>((string)redisValue);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default) where T : class
    {
        var jsonValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, jsonValue, expiry);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _database.KeyDeleteAsync(key);
    }
}
