namespace LubHub.Domain.Events
{
    /// <summary>
    /// Raised when a new streamer is registered
    /// </summary>
    public record StreamerCreatedEvent(string TwitchId) : IDomainEvent;
}