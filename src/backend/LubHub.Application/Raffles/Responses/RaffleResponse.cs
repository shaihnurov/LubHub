namespace LubHub.Application.Raffles.Responses;

/// <summary>
/// Response DTO representing a raffle public details
/// </summary>
/// <param name="Id">Unique identifier of the raffle</param>
/// <param name="Title">Title of the raffle</param>
/// <param name="Status">Current status of the raffle as a string</param>
/// <param name="CreatedAt">Timestamp when the raffle was created</param>
/// <param name="StartedAt">Timestamp when the raffle was started, null if not yet started</param>
/// <param name="EndedAt">Timestamp when the raffle was finished, null if not yet finished</param>
/// <param name="ParticipantCount">Number of registered participants</param>
public record RaffleResponse(int Id, int StreamerId, string StreamerName, string Title, string Status, DateTime CreatedAt, DateTime? StartedAt, DateTime? EndedAt, int ParticipantCount);