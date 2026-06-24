using LubHub.Application.Raffles.Responses;
using LubHub.Domain.Entities;
using Mapster;

namespace LubHub.Application.Common.Mappings;

/// <summary>
/// Mapster configuration for mapping <see cref="Raffle"/> to <see cref="RaffleResponse"/>
/// </summary>
public class RaffleResponseMappingConfig : IRegister
{
    /// <summary>
    /// Registers the mapping configuration
    /// </summary>
    /// <param name="config">The global Mapster configuration</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Raffle, RaffleResponse>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.StreamerId, src => src.StreamerId)
            .Map(dest => dest.StreamerName, src => src.Streamer != null ? src.Streamer.DisplayName : string.Empty)
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Status, src => src.Status.ToString())
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.StartedAt, src => src.StartedAt)
            .Map(dest => dest.EndedAt, src => src.EndedAt)
            .Map(dest => dest.ParticipantCount, src => src.Participants.Count());
    }
}