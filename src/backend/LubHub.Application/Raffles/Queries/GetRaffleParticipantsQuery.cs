using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Raffles.Responses;
using Mapster;
using MediatR;

namespace LubHub.Application.Raffles.Queries;

/// <summary>
/// Query to retrieve all participants of a specific raffle
/// </summary>
/// <param name="RaffleId">ID of the raffle</param>
public record GetRaffleParticipantsQuery(int RaffleId) : IRequest<IReadOnlyList<ParticipantResponse>>;

/// <summary>
/// Handles the <see cref="GetRaffleParticipantsQuery"/> request
/// </summary>
public class GetRaffleParticipantsQueryHandler(IRaffleRepository raffleRepository) : IRequestHandler<GetRaffleParticipantsQuery, IReadOnlyList<ParticipantResponse>>
{
    /// <summary>
    /// Retrieves all participants of the raffle and maps them to <see cref="ParticipantResponse"/>
    /// </summary>
    /// <param name="request">The query containing the raffle ID</param>
    /// <returns>Read-only list of participant responses</returns>
    public async Task<IReadOnlyList<ParticipantResponse>> Handle(GetRaffleParticipantsQuery request, CancellationToken ct)
    {
        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, ct)
            ?? throw new NotFoundException(nameof(RaffleResponse), request.RaffleId);

        return raffle.Participants.Adapt<IReadOnlyList<ParticipantResponse>>();
    }
}