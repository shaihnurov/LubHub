namespace LubHub.Domain.Entities
{
    /// <summary>
    /// Represents the winner of a raffle
    /// </summary>
    public class Winner : BaseEntity
    {
        /// <summary>
        /// ID of the raffle this winner belongs to
        /// </summary>
        public int RaffleId { get; private set; }

        /// <summary>
        /// ID of the participant who won
        /// </summary>
        public int ParticipantId { get; private set; }

        /// <summary>
        /// Twitch user ID of the winner
        /// </summary>
        public string TwitchUserId { get; private set; } = string.Empty;

        /// <summary>
        /// Display name of the winner on Twitch
        /// </summary>
        public string DisplayName { get; private set; } = string.Empty;

        /// <summary>
        /// Factory method to create a new winner
        /// </summary>
        /// <param name="raffleId">ID of the raffle</param>
        /// <param name="participantId">ID of the winning participant</param>
        /// <param name="twitchUserId">Twitch user ID of the winner</param>
        /// <param name="displayName">Display name of the winner</param>
        public static Winner Create(int raffleId, int participantId, string twitchUserId, string displayName)
        {
            return new Winner
            {
                RaffleId = raffleId,
                ParticipantId = participantId,
                TwitchUserId = twitchUserId,
                DisplayName = displayName
            };
        }
    }
}