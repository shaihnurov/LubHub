namespace LubHub.Application.Auth.Responses;

/// <summary>
/// Response returned after successful Twitch authentication
/// </summary>
/// <param name="Token">JWT token for subsequent API requests</param>
/// <param name="DisplayName">Twitch display name of the authenticated streamer</param>
/// <param name="TwitchId">Twitch user ID of the authenticated streamer</param>
public record AuthResponse(string Token, string DisplayName, string TwitchId);