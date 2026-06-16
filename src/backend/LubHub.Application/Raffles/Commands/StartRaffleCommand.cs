using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using MediatR;

namespace LubHub.Application.Raffles.Commands;

/// <summary>
/// Command to start a raffle, transitioning it from Pending to Active status
/// </summary>
/// <param name="TwitchId">Twitch ID of the streamer starting the raffle</param>
/// <param name="RaffleId">ID of the raffle to start</param>
public record StartRaffleCommand(string TwitchId, int RaffleId) : IRequest;

/// <summary>
/// Handles the <see cref="StartRaffleCommand"/> request
/// </summary>
public class StartRaffleCommandHandler(IStreamerRepository streamerRepository, IRaffleRepository raffleRepository) : IRequestHandler<StartRaffleCommand>
{
    /// <summary>
    /// Starts the raffle by transitioning it to Active status
    /// </summary>
    /// <param name="request">The command containing streamer and raffle identifiers</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown if the streamer or raffle is not found</exception>
    /// <exception cref="BusinessRuleException">Thrown if the raffle does not belong to the streamer</exception>
    public async Task Handle(StartRaffleCommand request, CancellationToken cancellationToken)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Raffle), request.RaffleId);

        if (raffle.StreamerId != streamer.Id)
            throw new BusinessRuleException("Raffle does not belong to this streamer");

        raffle.Start();
        await raffleRepository.UpdateAsync(raffle, cancellationToken);
    }
}