namespace LubHub.Domain.Enums
{
    /// <summary>
    /// Enumeration representing the different statuses of a raffle
    /// </summary>
    public enum RaffleStatus
    {
        /// <summary>
        /// Created but not yet started
        /// </summary>
        Pending = 0,
        /// <summary>
        /// Registration is open and participants can join
        /// </summary>
        Active = 1,
        /// <summary>
        /// Registration is closed and the raffle is finished
        /// </summary>
        Finished = 2,
        /// <summary>
        /// The raffle is finished and the winner has been drawn
        /// </summary>
        Drawn = 3
    }
}