namespace LubHub.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a domain rule is violated in a raffle operation
    /// </summary>
    public class RaffleDomainException(string message) : Exception(message) { }
}