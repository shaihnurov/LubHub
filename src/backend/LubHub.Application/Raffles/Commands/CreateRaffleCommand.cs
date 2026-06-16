using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using MediatR;

namespace LubHub.Application.Raffles.Commands;

/// <summary>
/// Command to create a new raffle for a streamer
/// </summary>
/// <param name="TwitchId">ID of the streamer creating the raffle</param>
/// <param name="Title">Title of the raffle</param>
public record CreateRaffleCommand(string TwitchId, string Title) : IRequest<int>;

/// <summary>
/// Handles the creation of a new raffle
/// </summary>
public class CreateRaffleCommandHandler(IRaffleRepository raffleRepository, IStreamerRepository streamerRepository) : IRequestHandler<CreateRaffleCommand, int>
{
    /// <summary>
    /// Handles the <see cref="CreateRaffleCommand"/> request
    /// </summary>
    /// <param name="request">The command containing raffle details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>ID of the newly created raffle</returns>
    /// <exception cref="InvalidOperationException">Thrown if streamer not found or already has an active raffle</exception>
    public async Task<int> Handle(CreateRaffleCommand request, CancellationToken cancellationToken)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, cancellationToken)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var hasActiveRaffle = await raffleRepository.IsActiveRaffleExistsAsync(streamer.Id, cancellationToken);

        if (hasActiveRaffle)
            throw new BusinessRuleException("Streamer already has an active raffle");

        var raffle = Raffle.Create(streamer.Id, request.Title);

        await raffleRepository.AddAsync(raffle, cancellationToken);
        return raffle.Id;
    }
}