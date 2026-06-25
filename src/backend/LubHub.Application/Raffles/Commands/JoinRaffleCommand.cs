using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Common.Messages;
using LubHub.Domain.Entities;
using LubHub.Domain.Enums;
using MediatR;

namespace LubHub.Application.Raffles.Commands;

/// <summary>
/// Command to register a viewer as a participant in an active raffle
/// </summary>
/// <param name="RaffleId">ID of the raffle to join</param>
/// <param name="TwitchUserId">Twitch ID of the viewer joining the raffle</param>
/// <param name="DisplayName">Display name of the viewer</param>
public record JoinRaffleCommand(int RaffleId, string TwitchUserId, string DisplayName) : IRequest;

/// <summary>
/// Handles the <see cref="JoinRaffleCommand"/> request
/// </summary>
public class JoinRaffleCommandHandler(IRaffleRepository raffleRepository, IRedisService redisService, IEventBus eventBus) : IRequestHandler<JoinRaffleCommand>
{
    /// <summary>
    /// Registers the viewer as a participant, using Redis for deduplication
    /// </summary>
    /// <param name="request">The command containing raffle and viewer details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown if the raffle is not found</exception>
    /// <exception cref="BusinessRuleException">Thrown if the raffle is not active or viewer already joined</exception>
    public async Task Handle(JoinRaffleCommand request, CancellationToken cancellationToken)
    {
        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, cancellationToken)
            ?? throw new NotFoundException(nameof(Raffle), request.RaffleId);

        if (raffle.Status != RaffleStatus.Active)
            throw new BusinessRuleException("Only active raffles can be joined.");

        if (raffle.StreamerId.ToString() == request.TwitchUserId)
            throw new BusinessRuleException("You cannot join your own raffle.");

        if (!await redisService.AddToSetAsync($"participants:{request.RaffleId}", request.TwitchUserId))
            throw new BusinessRuleException("Participant already joined this raffle.");

        await eventBus.PublishAsync(new JoinRaffleMessage(request.RaffleId, request.TwitchUserId, request.DisplayName), cancellationToken);
    }
}