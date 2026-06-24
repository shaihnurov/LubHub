using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Repository interface for raffle-specific operations
/// </summary>
public interface IRaffleRepository : IBaseRepository<Raffle>
{
    /// <summary>
    /// Gets all raffles created by a specific streamer
    /// </summary>
    /// <param name="streamerId">ID of the streamer</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Read-only list of raffles</returns>
    Task<IReadOnlyList<Raffle>> GetByStreamerIdAsync(int streamerId, CancellationToken ct = default);

    /// <summary>
    /// Checks whether a streamer already has an active raffle
    /// </summary>
    /// <param name="streamerId">ID of the streamer</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>True if an active raffle exists, otherwise false</returns>
    Task<bool> IsActiveRaffleExistsAsync(int streamerId, CancellationToken ct = default);

    /// <summary>
    /// Gets a public list of raffles with optional status filter and limit
    /// </summary>
    /// <param name="status">Optional status filter as string</param>
    /// <param name="limit">Optional maximum number of results</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Read-only list of raffles</returns>
    Task<IReadOnlyList<Raffle>> GetPublicListAsync(string? status, int? limit, CancellationToken ct = default);
}