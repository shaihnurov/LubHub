using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using LubHub.Application.Common.Interfaces;
using LubHub.Application.Common.Models;
using Microsoft.Extensions.Configuration;

namespace LubHub.Infrastructure.TwitchApi;

/// <summary>
/// Service for interacting with the Twitch OAuth and Helix APIs
/// </summary>
public class TwitchAuthService(HttpClient httpClient, IConfiguration configuration) : ITwitchAuthService
{
    private const string OAuthTokenEndpoint = "https://id.twitch.tv/oauth2/token";
    private const string TwitchHelixUsers = "https://api.twitch.tv/helix/users";

    /// <inheritdoc/>
    public async Task<string> ExchangeCodeForTokenAsync(string code, CancellationToken ct = default)
    {
        var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", configuration["Twitch:ClientId"]! },
            { "client_secret", configuration["Twitch:ClientSecret"]! },
            { "code", code },
            { "grant_type", "authorization_code" },
            { "redirect_uri", configuration["Twitch:RedirectUri"]! }
        });

        var response = await httpClient.PostAsync(OAuthTokenEndpoint, httpContent, ct);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TwitchTokenResponse>(ct)
            ?? throw new InvalidOperationException("Failed to deserialize Twitch token response.");

        return tokenResponse.AccessToken;
    }

    /// <inheritdoc/>
    public async Task<TwitchUserInfo> GetUserInfoAsync(string accessToken, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, TwitchHelixUsers);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("Client-Id", configuration["Twitch:ClientId"]!);

        var response = await httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();

        var userResponse = await response.Content.ReadFromJsonAsync<TwitchUsersResponse>(ct)
            ?? throw new InvalidOperationException("Failed to deserialize Twitch user info.");

        return userResponse.Data.FirstOrDefault()
            ?? throw new InvalidOperationException("No user data returned from Twitch API.");
    }

    /// <summary>
    /// Represents the token response from Twitch OAuth endpoint
    /// </summary>
    private record TwitchTokenResponse(
        [property: JsonPropertyName("access_token")] string AccessToken,
        [property: JsonPropertyName("token_type")] string TokenType
    );

    /// <summary>
    /// Wrapper for the Twitch Helix users endpoint response
    /// </summary>
    private record TwitchUsersResponse(
        [property: JsonPropertyName("data")] List<TwitchUserInfo> Data
    );
}