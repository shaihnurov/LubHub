using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using LubHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Repository for participant-specific database operations
/// </summary>
public class ParticipantRepository(AppDbContext dbContext) : BaseRepository<Participant>(dbContext), IParticipantRepository
{
    /// <inheritdoc/>
    public async Task<Participant?> GetByTwitchUserIdAsync(int raffleId, string twitchUserId, CancellationToken ct = default)
        => await DbContext.Participants.FirstOrDefaultAsync(p => p.RaffleId == raffleId && p.TwitchUserId == twitchUserId, ct);

    /// <inheritdoc/>
    public async Task<int> GetParticipatedCountAsync(string twitchUserId, CancellationToken ct = default)
        => await DbContext.Participants.CountAsync(p => p.TwitchUserId == twitchUserId, ct);
}