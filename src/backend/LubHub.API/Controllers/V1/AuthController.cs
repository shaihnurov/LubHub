using LubHub.Application.Auth.Commands;
using LubHub.Application.Auth.Responses;
using LubHub.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Controllers.V1;

/// <summary>
/// Controller for handling Twitch OAuth authentication
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    ITwitchAuthService twitchAuthService,
    ISender sender,
    IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// Returns the Twitch OAuth authorization URL for the login redirect
    /// </summary>
    /// <returns>Twitch OAuth URL</returns>
    [HttpGet("twitch/login")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult TwitchLogin()
        => Ok($"https://id.twitch.tv/oauth2/authorize" +
              $"?client_id={configuration["Twitch:ClientId"]}" +
              $"&redirect_uri={configuration["Twitch:RedirectUri"]}" +
              $"&response_type=code" +
              $"&scope=user:read:email");

    /// <summary>
    /// Handles the Twitch OAuth callback and returns a JWT token
    /// </summary>
    /// <param name="code">Authorization code received from Twitch</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>JWT token and streamer display name</returns>
    [HttpGet("twitch/callback")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> TwitchCallback([FromQuery] string code, CancellationToken ct)
    {
        var token = await twitchAuthService.ExchangeCodeForTokenAsync(code, ct);
        var userInfo = await twitchAuthService.GetUserInfoAsync(token, ct);

        var response = await sender.Send(new AuthenticateWithTwitchCommand(userInfo.Id, userInfo.DisplayName, userInfo.Email), ct);

        return Ok(response);
    }
}