namespace LubHub.Domain.Events
{
    /// <summary>
    /// Raised when a raffle is started
    /// </summary>
    public record RaffleStartedEvent(int RaffleId) : IDomainEvent;
}