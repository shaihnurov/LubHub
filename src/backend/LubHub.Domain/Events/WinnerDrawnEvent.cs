namespace LubHub.Domain.Events
{
    /// <summary>
    /// Raised when a winner is drawn from a finished raffle
    /// </summary>
    public record WinnerDrawnEvent(int RaffleId, string TwitchUserId) : IDomainEvent;
}