using LubHub.Application.Common.Interfaces;
using StackExchange.Redis;

namespace LubHub.Infrastructure.Services.Redis;

/// <summary>
/// Redis-based implementation of <see cref="IRedisService"/>
/// </summary>
public class RedisService(IConnectionMultiplexer redis) : IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();

    /// <inheritdoc/>
    public async Task<bool> AddToSetAsync(string key, string value)
        => await _db.SetAddAsync(key, value);

    /// <inheritdoc/>
    public async Task<string?> GetRandomFromSetAsync(string key)
    {
        var value = await _db.SetRandomMemberAsync(key);
        return value.IsNull ? null : value.ToString();
    }

    /// <inheritdoc/>
    public async Task<long> GetSetCountAsync(string key)
        => await _db.SetLengthAsync(key);
}