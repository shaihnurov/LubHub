using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using LubHub.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Repository for winner-specific database operations
/// </summary>
public class WinnerRepository(AppDbContext dbContext) : BaseRepository<Winner>(dbContext), IWinnerRepository
{
    /// <inheritdoc/>
    public async Task<int> GetWinsCountAsync(string twitchUserId, CancellationToken ct = default)
        => await DbContext.Winners.CountAsync(w => w.TwitchUserId == twitchUserId, ct);
}