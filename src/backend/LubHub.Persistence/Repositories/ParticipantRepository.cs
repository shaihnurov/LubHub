using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using LubHub.Persistence.Context;

namespace LubHub.Persistence.Repositories;

/// <summary>
/// Repository for participant-specific database operations
/// </summary>
public class ParticipantRepository(AppDbContext dbContext) : BaseRepository<Participant>(dbContext), IParticipantRepository;