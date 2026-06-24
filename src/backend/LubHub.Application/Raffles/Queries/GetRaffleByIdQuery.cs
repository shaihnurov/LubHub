using LubHub.Application.Common.Exceptions;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Raffles.Responses;
using Mapster;
using MediatR;

namespace LubHub.Application.Raffles.Queries;

/// <summary>
/// Query to retrieve a raffle by its ID
/// </summary>
/// <param name="RaffleId">ID of the raffle to retrieve</param>
public record GetRaffleByIdQuery(int RaffleId) : IRequest<RaffleResponse>;

/// <summary>
/// Handles the <see cref="GetRaffleByIdQuery"/> request
/// </summary>
public class GetRaffleByIdQueryHandler(IRaffleRepository raffleRepository) : IRequestHandler<GetRaffleByIdQuery, RaffleResponse>
{
    /// <summary>
    /// Retrieves the raffle and maps it to a <see cref="RaffleResponse"/>
    /// </summary>
    /// <param name="request">The query containing the raffle ID</param>
    /// <returns>Raffle details as a <see cref="RaffleResponse"/></returns>
    public async Task<RaffleResponse> Handle(GetRaffleByIdQuery request, CancellationToken ct)
    {
        var raffle = await raffleRepository.GetByIdAsync(request.RaffleId, ct)
            ?? throw new NotFoundException(nameof(RaffleResponse), request.RaffleId);

        return raffle.Adapt<RaffleResponse>();
    }
}