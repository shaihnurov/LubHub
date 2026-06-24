using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using LubHub.Domain.Enums;
using LubHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Repository for raffle-specific database operations
/// </summary>
public class RaffleRepository(AppDbContext dbContext) : BaseRepository<Raffle>(dbContext), IRaffleRepository
{
    /// <summary>
    /// Gets a raffle by its ID including its participants
    /// </summary>
    /// <param name="id">Raffle ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Raffle with participants if found, otherwise null</returns>
    public override async Task<Raffle?> GetByIdAsync(int id, CancellationToken ct = default)
        => await DbContext.Raffles.Include(x => x.Participants).FirstOrDefaultAsync(x => x.Id == id, ct);

    /// <summary>
    /// Gets all raffles created by a specific streamer
    /// </summary>
    /// <param name="streamerId">ID of the streamer</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Read-only list of raffles</returns>
    public async Task<IReadOnlyList<Raffle>> GetByStreamerIdAsync(int streamerId, CancellationToken ct = default)
        => await DbContext.Raffles.Include(x => x.Participants).Where(x => x.StreamerId == streamerId).ToListAsync(ct);

    /// <summary>
    /// Checks whether a streamer already has an active raffle
    /// </summary>
    /// <param name="streamerId">ID of the streamer</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if an active raffle exists, otherwise false</returns>
    public async Task<bool> IsActiveRaffleExistsAsync(int streamerId, CancellationToken ct = default)
        => await DbContext.Raffles.AnyAsync(x => x.StreamerId == streamerId && x.Status == RaffleStatus.Active, ct);
}