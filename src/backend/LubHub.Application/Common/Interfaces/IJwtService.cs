using LubHub.Domain.Entities;

namespace LubHub.Application.Common.Interfaces;

/// <summary>
/// Interface for JWT token generation
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the given streamer
    /// </summary>
    /// <param name="streamer">Streamer entity to generate token for</param>
    /// <returns>JWT token string</returns>
    string GenerateToken(Streamer streamer);
}