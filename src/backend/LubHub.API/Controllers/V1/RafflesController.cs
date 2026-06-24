using System.Security.Claims;
using Asp.Versioning;
using LubHub.API.Models.Requests;
using LubHub.Application.Common.Extensions;
using LubHub.Application.Raffles.Commands;
using LubHub.Application.Raffles.Queries;
using LubHub.Application.Raffles.Responses;
using LubHub.Application.Winners.Commands;
using LubHub.Application.Winners.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Controllers.V1;

/// <summary>
/// Controller for managing raffles
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RafflesController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Retrieves all participants of the specified raffle
    /// </summary>
    /// <param name="id">ID of the raffle</param>
    /// <returns>List of participants with bot scores</returns>
    [AllowAnonymous]
    [HttpGet("{id}/participants")]
    [ProducesResponseType(typeof(IReadOnlyList<ParticipantResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetRaffleParticipants(int id, CancellationToken cancellationToken)
    {
        var participants = await sender.Send(new GetRaffleParticipantsQuery(id), cancellationToken);
        return Ok(participants);
    }

    /// <summary>
    /// Retrieves the raffle history for the authenticated streamer
    /// </summary>
    /// <returns>List of raffles created by the streamer</returns>
    [HttpGet("my")]
    [ProducesResponseType(typeof(IReadOnlyList<RaffleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetStreamerHistory(CancellationToken cancellationToken)
    {
        var twitchId = User.GetTwitchId();
        var raffles = await sender.Send(new GetStreamerHistoryQuery(twitchId), cancellationToken);
        return Ok(raffles);
    }

    /// <summary>
    /// Retrieves a public list of all raffles with optional filtering
    /// </summary>
    /// <param name="status">Optional status filter: Pending, Active, Finished, Drawn</param>
    /// <param name="limit">Optional maximum number of raffles to return</param>
    /// <returns>List of raffles matching the filter</returns>
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<RaffleResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllRaffles([FromQuery] string? status, [FromQuery] int? limit, CancellationToken cancellationToken)
    {
        var raffles = await sender.Send(new GetPublicRafflesQuery(status, limit), cancellationToken);
        return Ok(raffles);
    }

    /// <summary>
    /// Retrieves a raffle by its ID without requiring authentication
    /// </summary>
    /// <param name="id">ID of the raffle to retrieve</param>
    /// <returns>Raffle details</returns>
    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RaffleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetRaffleById(int id, CancellationToken cancellationToken)
    {
        var raffle = await sender.Send(new GetRaffleByIdQuery(id), cancellationToken);
        return Ok(raffle);
    }

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
        await sender.Send(new StartRaffleCommand(twitchId, id), cancellationToken);
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
        await sender.Send(new FinishRaffleCommand(twitchId, id), cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Draws a winner for the specified raffle
    /// </summary>
    /// <param name="id">ID of the raffle to draw a winner for</param>
    /// <returns>Winner details</returns>
    [HttpPost("{id}/draw")]
    [ProducesResponseType(typeof(WinnerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DrawRaffle(int id, CancellationToken ct)
    {
        var twitchId = User.GetTwitchId();
        var winner = await sender.Send(new DrawWinnerCommand(twitchId, id), ct);
        return Ok(winner);
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
        var raffleId = await sender.Send(new CreateRaffleCommand(twitchId, request.Title), cancellationToken);
        return Created($"/api/v1/raffles/{raffleId}", raffleId);
    }

    /// <summary>
    /// Registers the authenticated viewer as a participant in the specified raffle
    /// </summary>
    /// <param name="id">ID of the raffle to join</param>
    /// <returns>Accepted</returns>
    [HttpPost("{id}/join")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> JoinRaffle(int id, CancellationToken cancellationToken)
    {
        var twitchUserId = User.GetTwitchId();
        var displayName = User.FindFirst(ClaimTypes.Name)!.Value;
        await sender.Send(new JoinRaffleCommand(id, twitchUserId, displayName), cancellationToken);
        return Accepted();
    }
}