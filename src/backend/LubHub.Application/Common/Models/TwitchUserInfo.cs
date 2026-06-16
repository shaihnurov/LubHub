using System.Text.Json.Serialization;

namespace LubHub.Application.Common.Models;

/// <summary>
/// Represents basic Twitch user information returned from the Twitch API
/// </summary>
/// <param name="Id">Twitch user ID</param>
/// <param name="Login">Twitch username</param>
/// <param name="DisplayName">Twitch display name</param>
/// <param name="Email">User email address</param>
public record TwitchUserInfo(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("display_name")] string DisplayName,
    [property: JsonPropertyName("email")] string Email
);