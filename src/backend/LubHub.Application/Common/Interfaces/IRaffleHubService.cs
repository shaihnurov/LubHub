namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Abstraction over SignalR hub for sending real-time raffle updates from the application layer
/// </summary>
public interface IRaffleHubService
{
    /// <summary>
    /// Sends the current participant count to all clients subscribed to the raffle group
    /// </summary>
    /// <param name="raffleId">ID of the raffle</param>
    /// <param name="count">Current number of registered participants</param>
    /// <param name="ct">Cancellation token</param>
    Task SendParticipantCountAsync(int raffleId, int count, CancellationToken ct = default);

    /// <summary>
    /// Sends the winner details to all clients subscribed to the raffle group
    /// </summary>
    /// <param name="raffleId">ID of the raffle</param>
    /// <param name="twitchUserId">Twitch user ID of the winner</param>
    /// <param name="displayName">Display name of the winner</param>
    /// <param name="ct">Cancellation token</param>
    Task SendWinnerAsync(int raffleId, string twitchUserId, string displayName, CancellationToken ct = default);

    /// <summary>
    /// Sends a join confirmation to the specific viewer who just registered
    /// </summary>
    /// <param name="twitchUserId">Twitch user ID of the viewer to notify</param>
    /// <param name="raffleId">ID of the raffle the viewer joined</param>
    /// <param name="ct">Cancellation token</param>
    Task SendJoinConfirmationAsync(string twitchUserId, int raffleId, CancellationToken ct = default);
}