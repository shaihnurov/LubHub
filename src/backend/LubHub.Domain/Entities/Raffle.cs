using LubHub.Domain.Enums;
using LubHub.Domain.Events;
using LubHub.Domain.Exceptions;

namespace LubHub.Domain.Entities
{
    /// <summary>
    /// Represents a raffle created by a streamer
    /// </summary>
    public class Raffle : BaseEntity
    {
        /// <summary>
        /// ID of the streamer who created this raffle
        /// </summary>
        public int StreamerId { get; private set; }

        /// <summary>
        /// Navigation property to the streamer who created this raffle
        /// </summary>
        public Streamer? Streamer { get; private set; }

        /// <summary>
        /// Title of the raffle
        /// </summary>
        public string Title { get; private set; } = string.Empty;

        /// <summary>
        /// Current status of the raffle
        /// </summary>
        public RaffleStatus Status { get; private set; }

        /// <summary>
        /// Timestamp when the raffle was started
        /// </summary>
        public DateTime? StartedAt { get; private set; }

        /// <summary>
        /// Timestamp when the raffle was finished
        /// </summary>
        public DateTime? EndedAt { get; private set; }

        private readonly List<Participant> _participants = [];

        /// <summary>
        /// Read-only collection of participants registered in this raffle
        /// </summary>
        public IReadOnlyCollection<Participant> Participants => _participants.AsReadOnly();

        /// <summary>
        /// Winner of the raffle, null until drawn
        /// </summary>
        public Winner? Winner { get; private set; }

        /// <summary>
        /// Factory method to create a new raffle
        /// </summary>
        /// <param name="streamerId">ID of the streamer creating the raffle</param>
        /// <param name="title">Title of the raffle</param>
        public static Raffle Create(int streamerId, string title)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(title);

            var raffle = new Raffle
            {
                StreamerId = streamerId,
                Title = title,
                Status = RaffleStatus.Pending
            };

            raffle.RaiseDomainEvent(new RaffleCreatedEvent(raffle.Id, streamerId));
            return raffle;
        }

        /// <summary>
        /// Starts the raffle, allowing participants to register
        /// </summary>
        /// <exception cref="RaffleDomainException">Thrown if raffle is not in Pending status</exception>
        public void Start()
        {
            if (Status != RaffleStatus.Pending)
                throw new RaffleDomainException("Raffle is already started or finished.");

            Status = RaffleStatus.Active;
            StartedAt = DateTime.UtcNow;

            RaiseDomainEvent(new RaffleStartedEvent(Id));
        }

        /// <summary>
        /// Finishes the raffle, closing participant registration
        /// </summary>
        /// <exception cref="RaffleDomainException">Thrown if raffle is not active</exception>
        public void Finish()
        {
            if (Status != RaffleStatus.Active)
                throw new RaffleDomainException("Cannot finish a raffle that is not active.");

            Status = RaffleStatus.Finished;
            EndedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Assigns the winner of the raffle
        /// </summary>
        /// <param name="winner">Winner entity to assign</param>
        /// <exception cref="RaffleDomainException">Thrown if raffle is not finished</exception>
        public void SetWinner(Winner winner)
        {
            if (Status != RaffleStatus.Finished)
                throw new RaffleDomainException("Winner can only be drawn after the raffle is finished.");

            Winner = winner;
            RaiseDomainEvent(new WinnerDrawnEvent(Id, winner.TwitchUserId));
        }
    }
}