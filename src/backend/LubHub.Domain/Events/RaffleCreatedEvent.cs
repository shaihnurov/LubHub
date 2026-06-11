namespace LubHub.Domain.Events
{
    /// <summary>
    /// Raised when a new raffle is created
    /// </summary>
    public record RaffleCreatedEvent(int RaffleId, int StreamerId) : IDomainEvent;
}