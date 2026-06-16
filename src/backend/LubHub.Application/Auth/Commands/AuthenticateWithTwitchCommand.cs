using LubHub.Application.Auth.Responses;
using LubHub.Application.Common.Interfaces;
using LubHub.Domain.Entities;
using MediatR;

namespace LubHub.Application.Auth.Commands;

/// <summary>
/// Command to authenticate a streamer via Twitch OAuth
/// </summary>
/// <param name="TwitchId">Twitch user ID</param>
/// <param name="DisplayName">Twitch display name</param>
/// <param name="Email">User email address</param>
public record AuthenticateWithTwitchCommand(string TwitchId, string DisplayName, string Email) : IRequest<AuthResponse>;

/// <summary>
/// Handles Twitch authentication by finding or creating a streamer and generating a JWT token
/// </summary>
public class AuthenticateWithTwitchCommandHandler(IStreamerRepository streamerRepository, IJwtService jwtService) : IRequestHandler<AuthenticateWithTwitchCommand, AuthResponse>
{
    /// <inheritdoc/>
    public async Task<AuthResponse> Handle(AuthenticateWithTwitchCommand request, CancellationToken cancellationToken)
    {
        var streamer = await streamerRepository.GetByTwitchIdAsync(request.TwitchId, cancellationToken);

        if (streamer is null)
        {
            streamer = Streamer.Create(request.TwitchId, request.DisplayName, request.Email);
            await streamerRepository.AddAsync(streamer, cancellationToken);
        }

        var token = jwtService.GenerateToken(streamer);
        return new AuthResponse(token, streamer.DisplayName);
    }
}