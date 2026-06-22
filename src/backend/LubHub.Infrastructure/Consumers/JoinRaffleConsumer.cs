using LubHub.Application.Common.Interfaces;
using LubHub.Application.Common.Messages;
using LubHub.Domain.Entities;
using MassTransit;

namespace LubHub.Infrastructure.Consumers;

/// <summary>
/// Consumes <see cref="JoinRaffleMessage"/> and persists the participant to the database
/// </summary>
public class JoinRaffleConsumer(IParticipantRepository participantRepository, IRaffleHubService raffleHubService, IRedisService redisService) : IConsumer<JoinRaffleMessage>
{
    /// <inheritdoc/>
    public async Task Consume(ConsumeContext<JoinRaffleMessage> context)
    {
        var message = context.Message;
        var participant = Participant.Create(message.RaffleId, message.TwitchUserId, message.DisplayName);
        await participantRepository.AddAsync(participant, context.CancellationToken);

        var count = await redisService.GetSetCountAsync($"participants:{message.RaffleId}");
        await raffleHubService.SendParticipantCountAsync(message.RaffleId, (int)count, context.CancellationToken);
        await raffleHubService.SendJoinConfirmationAsync(message.TwitchUserId, message.RaffleId, context.CancellationToken);
    }
}