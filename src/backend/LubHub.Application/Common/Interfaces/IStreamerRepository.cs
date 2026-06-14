using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Repository interface for streamer-specific operations
/// </summary>
public interface IStreamerRepository : IBaseRepository<Streamer>
{
    /// <summary>
    /// Gets a streamer by their Twitch ID
    /// </summary>
    /// <param name="twitchId">Twitch ID of the streamer</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Streamer if found, otherwise null</returns>
    Task<Streamer?> GetByTwitchIdAsync(string twitchId, CancellationToken ct = default);
}