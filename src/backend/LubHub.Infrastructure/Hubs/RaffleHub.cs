using LubHub.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LubHub.Infrastructure.Hubs;

/// <summary>
/// SignalR hub for real-time raffle updates
/// </summary>
[Authorize]
public class RaffleHub : Hub<IRaffleHub>
{
    /// <summary>
    /// Adds the caller to the SignalR group for the specified raffle
    /// </summary>
    /// <param name="raffleId">ID of the raffle to subscribe to</param>
    public async Task JoinRaffleGroup(int raffleId)
        => await Groups.AddToGroupAsync(Context.ConnectionId, $"raffle-{raffleId}");

    /// <summary>
    /// Removes the caller from the SignalR group for the specified raffle
    /// </summary>
    /// <param name="raffleId">ID of the raffle to unsubscribe from</param>
    public async Task LeaveRaffleGroup(int raffleId)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"raffle-{raffleId}");
}