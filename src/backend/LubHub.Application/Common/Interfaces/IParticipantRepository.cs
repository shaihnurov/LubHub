using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Repository interface for participant-specific database operations
/// </summary>
public interface IParticipantRepository : IBaseRepository<Participant>
{
    /// <summary>
    /// Gets a participant by their Twitch user ID within a specific raffle
    /// </summary>
    /// <param name="raffleId">ID of the raffle</param>
    /// <param name="twitchUserId">Twitch user ID of the participant</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Participant if found, otherwise null</returns>
    Task<Participant?> GetByTwitchUserIdAsync(int raffleId, string twitchUserId, CancellationToken ct = default);
}