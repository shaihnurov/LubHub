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
        => await DbContext.Raffles.Include(x => x.Participants).Include(x => x.Streamer).FirstOrDefaultAsync(x => x.Id == id, ct);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Raffle>> GetByStreamerIdAsync(int streamerId, CancellationToken ct = default)
        => await DbContext.Raffles.Include(x => x.Participants).Include(x => x.Streamer).Where(x => x.StreamerId == streamerId).ToListAsync(ct);

    /// <inheritdoc/>
    public async Task<bool> IsActiveRaffleExistsAsync(int streamerId, CancellationToken ct = default)
        => await DbContext.Raffles.AnyAsync(x => x.StreamerId == streamerId && x.Status == RaffleStatus.Active, ct);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Raffle>> GetPublicListAsync(string? status, int? limit, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Raffles.Include(x => x.Participants).Include(x => x.Streamer).AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<RaffleStatus>(status, out var raffleStatus))
            query = query.Where(x => x.Status == raffleStatus);

        if (limit.HasValue)
            query = query.Take(limit.Value);

        return await query.ToListAsync(cancellationToken);
    }
}