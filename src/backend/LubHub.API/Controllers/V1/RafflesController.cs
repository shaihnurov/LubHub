using LubHub.API.Models.Requests;
using LubHub.Application.Common.Extensions;
using LubHub.Application.Raffles.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Controllers.V1;

/// <summary>
/// Controller for managing raffles
/// </summary>
[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class RafflesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Starts an existing raffle, opening participant registration
    /// </summary>
    /// <param name="id">ID of the raffle to start</param>
    /// <returns>No content on success</returns>
    [HttpPost("{id}/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> StartRaffle(int id, CancellationToken cancellationToken)
    {
        var twitchId = User.GetTwitchId();
        var command = new StartRaffleCommand(twitchId, id);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Finishes an existing raffle, closing participant registration
    /// </summary>
    /// <param name="id">ID of the raffle to finish</param>
    /// <returns>No content on success</returns>
    [HttpPost("{id}/finish")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> FinishRaffle(int id, CancellationToken cancellationToken)
    {
        var twitchId = User.GetTwitchId();
        var command = new FinishRaffleCommand(twitchId, id);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Creates a new raffle
    /// </summary>
    /// <param name="request">Raffle creation request</param>
    /// <returns>ID of the created raffle</returns>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateRaffle([FromBody] CreateRaffleRequest request, CancellationToken cancellationToken)
    {
        var twitchId = User.GetTwitchId();
        var command = new CreateRaffleCommand(twitchId, request.Title);
        var raffleId = await sender.Send(command, cancellationToken);
        return Created($"/api/v1/raffles/{raffleId}", raffleId);
    }
}