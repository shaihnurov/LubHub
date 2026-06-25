using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Repository interface for winner-specific database operations
/// </summary>
public interface IWinnerRepository : IBaseRepository<Winner>
{
    /// <summary>
    /// Returns the total number of raffles the user has won
    /// </summary>
    /// <param name="twitchUserId">Twitch user ID</param>
    /// <param name="ct">Cancellation token</param>
    Task<int> GetWinsCountAsync(string twitchUserId, CancellationToken ct = default);
}