using LubHub.Application.Common.Interfaces;
using StackExchange.Redis;

namespace LubHub.Infrastructure.Services.Redis;

/// <summary>
/// Redis-based implementation of <see cref="IRedisService"/>
/// </summary>
public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();

    /// <summary>
    /// Adds a value to a Redis set. Returns false if the value already exists in the set
    /// </summary>
    /// <param name="key">Redis set key</param>
    /// <param name="value">Value to add</param>
    /// <returns>True if added, false if already present</returns>
    public async Task<bool> AddToSetAsync(string key, string value)
        => await _db.SetAddAsync(key, value);
}