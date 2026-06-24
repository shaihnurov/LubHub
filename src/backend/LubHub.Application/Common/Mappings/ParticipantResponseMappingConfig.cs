using LubHub.Application.Raffles.Responses;
using LubHub.Domain.Entities;
using Mapster;

namespace LubHub.Application.Common.Mappings;

/// <summary>
/// Mapster configuration for mapping <see cref="Participant"/> to <see cref="ParticipantResponse"/>
/// </summary>
public class ParticipantResponseMappingConfig : IRegister
{
    /// <summary>
    /// Registers the mapping configuration
    /// </summary>
    /// <param name="config">The global Mapster configuration</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Participant, ParticipantResponse>();
    }
}