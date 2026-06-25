namespace LubHub.Application.Profile.Responses;

/// <summary>
/// Response DTO representing the authenticated user's profile and statistics
/// </summary>
/// <param name="TwitchId">Twitch user ID of the streamer</param>
/// <param name="DisplayName">Display name on Twitch</param>
/// <param name="Email">Email address of the streamer</param>
/// <param name="RafflesCreated">Total number of raffles created by the streamer</param>
/// <param name="RafflesParticipated">Total number of raffles the user participated in as a viewer</param>
/// <param name="Wins">Total number of raffles won</param>
/// <param name="CreatedAt">Timestamp when the account was created</param>
public record ProfileResponse(string TwitchId, string DisplayName, string Email, int RafflesCreated, int RafflesParticipated, int Wins, DateTime CreatedAt);