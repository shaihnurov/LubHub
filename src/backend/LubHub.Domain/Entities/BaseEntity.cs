using LubHub.Domain.Events;

namespace LubHub.Domain.Entities
{
    /// <summary>
    /// Base entity class that includes common properties and domain event handling
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Unique identifier for the entity
        /// </summary>
        public int Id { get; protected set; }
        /// <summary>
        /// Timestamp indicating when the entity was created
        /// </summary>
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

        /// <summary>
        /// List of domain events that have occurred on this entity. 
        /// This allows the entity to raise events that can be handled by other parts of the system, such as application services or event handlers 
        /// </summary>
        private readonly List<IDomainEvent> _domainEvents = [];

        /// <summary>
        /// Collections of domain events that have been raised by this entity
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        /// <summary>
        /// Raises a domain event and adds it to the list of domain events for this entity
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

        /// <summary>
        /// Clears all domain events from the entity
        /// </summary>
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}