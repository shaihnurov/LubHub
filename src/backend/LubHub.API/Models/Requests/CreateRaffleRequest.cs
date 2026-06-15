namespace LubHub.API.Models.Requests;

/// <summary>
/// Request model for creating a new raffle
/// </summary>
/// <param name="Title">Title of the raffle</param>
public record CreateRaffleRequest(string Title);