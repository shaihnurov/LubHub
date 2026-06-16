using LubHub.Application.Common.Interfaces;
using LubHub.Infrastructure.Auth;
using LubHub.Infrastructure.TwitchApi;
using Microsoft.Extensions.DependencyInjection;

namespace LubHub.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering Infrastructure layer services
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services into the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient<ITwitchAuthService, TwitchAuthService>();
        services.AddScoped<IJwtService, JwtService>();
        return services;
    }
}