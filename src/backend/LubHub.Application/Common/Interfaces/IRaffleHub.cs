namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Typed SignalR client interface defining methods callable on connected raffle clients
/// </summary>
public interface IRaffleHub
{
    /// <summary>
    /// Sends the updated participant count to the client
    /// </summary>
    /// <param name="count">Current number of registered participants</param>
    Task ParticipantCountUpdated(int count);

    /// <summary>
    /// Sends the winner details to the client after the draw
    /// </summary>
    /// <param name="twitchUserId">Twitch user ID of the winner</param>
    /// <param name="displayName">Display name of the winner</param>
    Task WinnerDrawn(string twitchUserId, string displayName);

    /// <summary>
    /// Notifies the client that their raffle registration was successful
    /// </summary>
    /// <param name="raffleId">ID of the raffle the client joined</param>
    Task JoinConfirmed(int raffleId);
}