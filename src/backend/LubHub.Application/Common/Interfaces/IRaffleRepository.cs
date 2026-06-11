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
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Read-only list of raffles</returns>
    Task<IReadOnlyList<Raffle>> GetByStreamerIdAsync(int streamerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether a streamer already has an active raffle
    /// </summary>
    /// <param name="streamerId">ID of the streamer</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if an active raffle exists, otherwise false</returns>
    Task<bool> IsActiveRaffleExistsAsync(int streamerId, CancellationToken cancellationToken = default);
}