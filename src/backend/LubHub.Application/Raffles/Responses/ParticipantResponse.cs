namespace LubHub.Application.Raffles.Responses;

/// <summary>
/// Response DTO representing a participant in a raffle
/// </summary>
/// <param name="Id">Unique identifier of the participant</param>
/// <param name="TwitchUserId">Twitch user ID of the participant</param>
/// <param name="DisplayName">Display name of the participant</param>
/// <param name="BotScore">Bot probability score from 0.0 (human) to 1.0 (bot)</param>
/// <param name="IsSuspected">True if the participant is suspected to be a bot</param>
public record ParticipantResponse(int Id, string TwitchUserId, string DisplayName, float BotScore, bool IsSuspected);