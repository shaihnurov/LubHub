using LubHub.API.Models.Requests;
using LubHub.Application.Raffles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Controllers.V1;

/// <summary>
/// Controller for managing raffles
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class RafflesController(ISender sender) : ControllerBase
{
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
        var streamerId = int.Parse(User.FindFirst("sub")!.Value);
        var command = new CreateRaffleCommand(streamerId, request.Title);
        var raffleId = await sender.Send(command, cancellationToken);
        return Created($"/api/v1/raffles/{raffleId}", raffleId);
    }
}