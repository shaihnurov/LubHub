using LubHub.Domain.Events;

namespace LubHub.Domain.Entities
{
    /// <summary>
    /// Represents a streamer who can create raffles and have participants
    /// </summary>
    public class Streamer : BaseEntity
    {
        /// <summary>
        /// Twitch ID of the streamer, used to link with Twitch API and identify the streamer uniquely
        /// </summary>
        public string TwitchId { get; private set; } = string.Empty;
        /// <summary>
        /// Display name of the streamer
        /// </summary>
        public string DisplayName { get; private set; } = string.Empty;
        /// <summary>
        /// Email address of the streamer
        /// </summary>
        public string Email { get; private set; } = string.Empty;

        /// <summary>
        /// Collection of raffles created by the streamer
        /// </summary>
        private readonly List<Raffle> _raffles = [];
        /// <summary>
        /// Read-only collection of raffles created by the streamer
        /// </summary>
        public IReadOnlyCollection<Raffle> Raffles => _raffles.AsReadOnly();

        /// <summary>
        /// Factory method to create a new Streamer instance
        /// </summary>
        /// <param name="twitchId">The Twitch ID of the streamer</param>
        /// <param name="displayName">The display name of the streamer</param>
        /// <param name="email">The email address of the streamer</param>
        /// <returns>The created Streamer instance</returns>
        public static Streamer Create(string twitchId, string displayName, string email)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(twitchId);
            ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

            var streamer = new Streamer
            {
                TwitchId = twitchId,
                DisplayName = displayName,
                Email = email
            };

            streamer.RaiseDomainEvent(new StreamerCreatedEvent(streamer.TwitchId));
            return streamer;
        }
    }
}