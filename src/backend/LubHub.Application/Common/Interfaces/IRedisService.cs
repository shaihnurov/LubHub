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
}