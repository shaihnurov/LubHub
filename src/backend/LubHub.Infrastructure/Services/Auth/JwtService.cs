using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LubHub.Infrastructure.Services.Auth;

/// <summary>
/// Service for generating JWT tokens for authenticated streamers
/// </summary>
public class JwtService(IConfiguration configuration) : IJwtService
{
    /// <inheritdoc/>
    public string GenerateToken(Streamer streamer)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expirationDays = int.Parse(configuration["Jwt:ExpirationDays"]!);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, streamer.TwitchId.ToString()),
            new Claim(ClaimTypes.Name, streamer.DisplayName),
            new Claim(ClaimTypes.Email, streamer.Email)
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"]!,
            audience: configuration["Jwt:Audience"]!,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expirationDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}