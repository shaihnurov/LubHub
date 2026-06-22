using LubHub.Application.Common.Interfaces;
using LubHub.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LubHub.Infrastructure.Services.SignalR;

/// <summary>
/// SignalR-based implementation of <see cref="IRaffleHubService"/>
/// </summary>
public class RaffleHubService(IHubContext<RaffleHub, IRaffleHub> hubContext) : IRaffleHubService
{
    /// <inheritdoc/>
    public async Task SendParticipantCountAsync(int raffleId, int count, CancellationToken ct = default)
        => await hubContext.Clients.Group($"raffle-{raffleId}").ParticipantCountUpdated(count);

    /// <inheritdoc/>
    public async Task SendWinnerAsync(int raffleId, string twitchUserId, string displayName, CancellationToken ct = default)
        => await hubContext.Clients.Group($"raffle-{raffleId}").WinnerDrawn(twitchUserId, displayName);

    /// <inheritdoc/>
    public async Task SendJoinConfirmationAsync(string twitchUserId, int raffleId, CancellationToken ct = default)
        => await hubContext.Clients.User(twitchUserId).JoinConfirmed(raffleId);
}