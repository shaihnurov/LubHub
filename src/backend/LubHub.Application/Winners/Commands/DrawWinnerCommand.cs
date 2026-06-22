using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Winners.Responses;
using LubHub.Domain.Entities;
using LubHub.Domain.Enums;
using MediatR;

namespace LubHub.Application.Winners.Commands;

/// <summary>
/// Command to draw a winner from the registered participants of a finished raffle
/// </summary>
/// <param name="TwitchId">Twitch ID of the streamer drawing the winner</param>
/// <param name="RaffleId">ID of the raffle to draw a winner for</param>
public record DrawWinnerCommand(string TwitchId, int RaffleId) : IRequest<WinnerResponse>;

/// <summary>
/// Handles the <see cref="DrawWinnerCommand"/> request
/// </summary>
public class DrawWinnerCommandHandler(IRaffleRepository raffleRepository, IStreamerRepository streamerRepository, IParticipantRepository participantRepository,
    IRedisService redis, IRaffleHubService raffleHubService) : IRequestHandler<DrawWinnerCommand, WinnerResponse>
{
    /// <summary>
    /// Draws a random winner from Redis, persists the result and returns winner details
    /// </summary>
    /// <param name="request">The command containing streamer and raffle identifiers</param>
    /// <returns>Winner details including Twitch user ID and display name</returns>
    public async Task<WinnerResponse> Handle(DrawWinnerCommand request, CancellationToken ct)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, ct)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, ct)
            ?? throw new NotFoundException(nameof(Raffle), request.RaffleId);

        if (raffle.StreamerId != streamer.Id)
            throw new BusinessRuleException("Raffle does not belong to this streamer");

        if (raffle.Status != RaffleStatus.Finished)
            throw new BusinessRuleException("The drawing is not over yet");

        var winnerTwitchId = await redis.GetRandomFromSetAsync($"participants:{request.RaffleId}")
            ?? throw new BusinessRuleException("Unable to find the winner's Twitch User ID");

        var member = await participantRepository.GetByTwitchUserIdAsync(raffle.Id, winnerTwitchId, ct)
            ?? throw new NotFoundException(nameof(Participant), winnerTwitchId);

        var winner = Winner.Create(raffle.Id, member.Id, winnerTwitchId, member.DisplayName);

        raffle.SetWinner(winner);
        await raffleRepository.UpdateAsync(raffle, ct);
        await raffleHubService.SendWinnerAsync(raffle.Id, winnerTwitchId, member.DisplayName, ct);

        return new WinnerResponse(member.Id, winnerTwitchId, member.DisplayName);
    }
}