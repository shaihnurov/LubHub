using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Profile.Responses;
using LubHub.Domain.Entities;
using MediatR;

namespace LubHub.Application.Profile.Queries;

/// <summary>
/// Query to retrieve the profile of the authenticated streamer
/// </summary>
/// <param name="TwitchId">Twitch ID of the streamer</param>
public record GetProfileQuery(string TwitchId) : IRequest<ProfileResponse>;

/// <summary>
/// Handles the <see cref="GetProfileQuery"/> request
/// </summary>
public class GetProfileQueryHandler(IStreamerRepository streamerRepository, IParticipantRepository participantRepository, 
    IWinnerRepository winnerRepository, IRaffleRepository raffleRepository) : IRequestHandler<GetProfileQuery, ProfileResponse>
{
    /// <summary>
    /// Retrieves the streamer profile with participation statistics
    /// </summary>
    /// <param name="request">The query containing the streamer's Twitch ID</param>
    /// <returns>Profile response with statistics</returns>
    public async Task<ProfileResponse> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, ct)
            ?? throw new NotFoundException(nameof(Streamer), request.TwitchId);

        var rafflesCreated = await raffleRepository.GetCountAsync(streamer.Id, ct);
        var participatedCount = await participantRepository.GetParticipatedCountAsync(request.TwitchId, ct);
        var winsCount = await winnerRepository.GetWinsCountAsync(request.TwitchId, ct);

        return new ProfileResponse(streamer.TwitchId, streamer.DisplayName, streamer.Email, rafflesCreated, participatedCount, winsCount, streamer.CreatedAt);
    }
}