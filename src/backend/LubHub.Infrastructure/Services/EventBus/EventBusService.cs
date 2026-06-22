using LubHub.Application.Common.Interfaces;
using MassTransit;

namespace LubHub.Infrastructure.Services.EventBus;

/// <summary>
/// MassTransit-based implementation of <see cref="IEventBus"/>
/// </summary>
public class EventBusService(IPublishEndpoint publishEndpoint) : IEventBus
{
    /// <inheritdoc/>
    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class
        => await publishEndpoint.Publish(message, cancellationToken);
}