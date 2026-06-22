namespace LubHub.Application.Winners.Responses;

/// <summary>
/// Response DTO returned after a winner is drawn for a raffle
/// </summary>
/// <param name="WinnerId">Database ID of the winning participant</param>
/// <param name="TwitchUserId">Twitch user ID of the winner</param>
/// <param name="DisplayName">Display name of the winner</param>
public record WinnerResponse(int WinnerId, string TwitchUserId, string DisplayName);