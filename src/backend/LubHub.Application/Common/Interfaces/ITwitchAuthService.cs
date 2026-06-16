using LubHub.Application.Common.Models;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Interface for Twitch OAuth authentication operations
/// </summary>
public interface ITwitchAuthService
{
    /// <summary>
    /// Exchanges an authorization code for a Twitch access token
    /// </summary>
    /// <param name="code">Authorization code received from Twitch callback</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Twitch access token</returns>
    Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default);

    /// <summary>
    /// Gets the authenticated Twitch user's information
    /// </summary>
    /// <param name="accessToken">Twitch access token</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Twitch user information</returns>
    Task<TwitchUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct = default);
}