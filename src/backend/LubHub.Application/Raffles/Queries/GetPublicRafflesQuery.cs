using LubHub.Application.Common.Interfaces;
using LubHub.Application.Raffles.Responses;
using Mapster;
using MediatR;

namespace LubHub.Application.Raffles.Queries;

/// <summary>
/// Query to retrieve a public list of raffles with optional status filter and limit
/// </summary>
/// <param name="Status">Optional status filter</param>
/// <param name="Limit">Optional maximum number of results</param>
public record GetPublicRafflesQuery(string? Status, int? Limit) : IRequest<IReadOnlyList<RaffleResponse>>;

/// <summary>
/// Handles the <see cref="GetPublicRafflesQuery"/> request
/// </summary>
public class GetPublicRafflesQueryHandler(IRaffleRepository raffleRepository) : IRequestHandler<GetPublicRafflesQuery, IReadOnlyList<RaffleResponse>>
{
    /// <summary>
    /// Retrieves all public raffles applying optional filters and maps them to <see cref="RaffleResponse"/>
    /// </summary>
    /// <param name="request">The query containing optional status and limit filters</param>
    /// <returns>Read-only list of raffle responses</returns>
    public async Task<IReadOnlyList<RaffleResponse>> Handle(GetPublicRafflesQuery request, CancellationToken ct)
    {
        var raffles = await raffleRepository.GetPublicListAsync(request.Status, request.Limit, ct);
        return raffles.Adapt<IReadOnlyList<RaffleResponse>>();
    }
}