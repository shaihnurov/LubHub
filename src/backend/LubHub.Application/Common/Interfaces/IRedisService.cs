namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Abstraction over Redis operations used by the application layer
/// </summary>
public interface IRedisService
{
    /// <summary>
    /// Adds a value to a Redis set
    /// </summary>
    /// <param name="key">Redis set key</param>
    /// <param name="value">Value to add</param>
    /// <returns>True if added successfully, false if the value already existed in the set</returns>
    Task<bool> AddToSetAsync(string key, string value);

    /// <summary>
    /// Returns a random value from a Redis set without removing it
    /// </summary>
    /// <param name="key">Redis set key</param>
    /// <returns>Random value from the set, or null if the set is empty or does not exist</returns>
    Task<string?> GetRandomFromSetAsync(string key);

    /// <summary>
    /// Returns the number of members in a Redis set
    /// </summary>
    /// <param name="key">Redis set key</param>
    Task<long> GetSetCountAsync(string key);
}