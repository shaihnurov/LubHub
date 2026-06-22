namespace LubHub.Application.Common.Messages;

/// <summary>
/// Message published when a viewer successfully joins a raffle
/// </summary>
/// <param name="RaffleId">ID of the raffle</param>
/// <param name="TwitchUserId">Twitch user ID of the participant</param>
/// <param name="DisplayName">Display name of the participant</param>
public record JoinRaffleMessage(int RaffleId, string TwitchUserId, string DisplayName);