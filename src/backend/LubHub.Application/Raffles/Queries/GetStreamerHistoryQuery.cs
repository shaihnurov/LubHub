using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Raffles.Responses;
using LubHub.Domain.Entities;
using Mapster;
using MediatR;

namespace LubHub.Application.Raffles.Queries;

/// <summary>
/// Query to retrieve all raffles created by the authenticated streamer
/// </summary>
/// <param name="TwitchId">Twitch ID of the streamer</param>
public record GetStreamerHistoryQuery(string TwitchId) : IRequest<IReadOnlyList<RaffleResponse>>;

/// <summary>
/// Handles the <see cref="GetStreamerHistoryQuery"/> request
/// </summary>
public class GetStreamerHistoryQueryHandler(IRaffleRepository raffleRepository, IStreamerRepository streamerRepository)
    : IRequestHandler<GetStreamerHistoryQuery, IReadOnlyList<RaffleResponse>>
{
    /// <summary>
    /// Retrieves all raffles for the streamer and maps them to <see cref="RaffleResponse"/>
    /// </summary>
    /// <param name="request">The query containing the streamer's Twitch ID</param>
    /// <returns>Read-only list of raffle responses</returns>
    public async Task<IReadOnlyList<RaffleResponse>> Handle(GetStreamerHistoryQuery request, CancellationToken ct)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, ct)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var raffles = await raffleRepository.GetByStreamerIdAsync(streamer.Id, ct);

        return raffles.Adapt<IReadOnlyList<RaffleResponse>>();
    }
}