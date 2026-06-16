using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Repository interface for participant-specific database operations
/// </summary>
public interface IParticipantRepository : IBaseRepository<Participant>
{
}