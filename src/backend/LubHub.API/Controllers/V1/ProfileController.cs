using Asp.Versioning;
using LubHub.Application.Common.Extensions;
using LubHub.Application.Profile.Queries;
using LubHub.Application.Profile.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Controllers.V1;

/// <summary>
/// Controller for managing user profile information
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProfileController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Retrieves the authenticated user profile statistics
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation</param>
    /// <returns>User profile statistics as ProfileResponse</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProfileStats(CancellationToken cancellationToken)
    {
        var twitchId = User.GetTwitchId();
        var raffles = await sender.Send(new GetProfileQuery(twitchId), cancellationToken);
        return Ok(raffles);
    }
}