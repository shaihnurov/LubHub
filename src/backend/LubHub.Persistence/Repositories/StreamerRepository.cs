using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using LubHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Repository for streamer-specific database operations
/// </summary>
public class StreamerRepository(AppDbContext dbContext) : BaseRepository<Streamer>(dbContext), IStreamerRepository
{
    /// <summary>
    /// Gets a streamer by their Twitch ID
    /// </summary>
    /// <param name="twitchId">Twitch ID of the streamer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Streamer if found, otherwise null</returns>
    public async Task<Streamer?> GetByTwitchIdAsync(string twitchId, CancellationToken cancellationToken = default)
        => await DbContext.Streamers.FirstOrDefaultAsync(x => x.TwitchId == twitchId, cancellationToken);
}