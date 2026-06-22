namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the message broker used by the application layer
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes a message to the message broker
    /// </summary>
    /// <param name="message">Message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : class;
}