namespace LubHub.Domain.Entities
{
    /// <summary>
    /// Represents a viewer who registered for a raffle
    /// </summary>
    public class Participant : BaseEntity
    {
        /// <summary>
        /// ID of the raffle this participant belongs to
        /// </summary>
        public int RaffleId { get; private set; }

        /// <summary>
        /// Twitch user ID of the participant
        /// </summary>
        public string TwitchUserId { get; private set; } = string.Empty;

        /// <summary>
        /// Display name of the participant on Twitch
        /// </summary>
        public string DisplayName { get; private set; } = string.Empty;

        /// <summary>
        /// Bot likelihood score from 0.0 (human) to 1.0 (maybe bot)
        /// </summary>
        public float BotScore { get; private set; }

        /// <summary>
        /// True if the participant is suspected to be a bot
        /// </summary>
        public bool IsSuspected => BotScore >= 0.7f;

        /// <summary>
        /// Factory method to create a new participant
        /// </summary>
        /// <param name="raffleId">ID of the raffle to join</param>
        /// <param name="twitchUserId">Twitch user ID of the participant</param>
        /// <param name="displayName">Display name of the participant</param>
        /// <param name="botScore">Bot likelihood score, defaults to 0</param>
        public static Participant Create(int raffleId, string twitchUserId, string displayName, float botScore = 0f)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(twitchUserId);

            return new Participant
            {
                RaffleId = raffleId,
                TwitchUserId = twitchUserId,
                DisplayName = displayName,
                BotScore = botScore
            };
        }
    }
}