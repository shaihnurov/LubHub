using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using MediatR;

namespace LubHub.Application.Raffles.Commands;

/// <summary>
/// Command to finish a raffle, transitioning it from Active to Finished status
/// </summary>
/// <param name="TwitchId">Twitch ID of the streamer finishing the raffle</param>
/// <param name="RaffleId">ID of the raffle to finish</param>
public record FinishRaffleCommand(string TwitchId, int RaffleId) : IRequest;

/// <summary>
/// Handles the <see cref="FinishRaffleCommand"/> request
/// </summary>
public class FinishRaffleCommandHandler(IStreamerRepository streamerRepository, IRaffleRepository raffleRepository) : IRequestHandler<FinishRaffleCommand>
{
    /// <summary>
    /// Finishes the raffle by transitioning it to Finished status
    /// </summary>
    /// <param name="request">The command containing streamer and raffle identifiers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown if the streamer or raffle is not found</exception>
    /// <exception cref="BusinessRuleException">Thrown if the raffle does not belong to the streamer</exception>
    public async Task Handle(FinishRaffleCommand request, CancellationToken cancellationToken)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Raffle), request.RaffleId);

        if (raffle.StreamerId != streamer.Id)
            throw new BusinessRuleException("Raffle does not belong to this streamer");

        raffle.Finish();
        await raffleRepository.UpdateAsync(raffle, cancellationToken);
    }
}