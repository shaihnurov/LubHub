using System.Security.Claims;
using LubHub.Application.Common.Exceptions;

namespace LubHub.Application.Common.Extensions;

/// <summary>
/// Extension methods for <see cref="ClaimsPrincipal"/>
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the Twitch ID from the JWT sub claim
    /// </summary>
    /// <param name="principal">The claims principal from the current HTTP context</param>
    /// <returns>Twitch ID string</returns>
    /// <exception cref="UnauthorizedException">Thrown if the sub claim is missing</exception>
    public static string GetTwitchId(this ClaimsPrincipal principal)
        => principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedException("Twitch ID not found in claims");
}