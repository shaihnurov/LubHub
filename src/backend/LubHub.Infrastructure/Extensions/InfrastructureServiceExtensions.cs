using LubHub.Application.Common.Interfaces;
using LubHub.Infrastructure.Services.Auth;
using LubHub.Infrastructure.Services.Redis;
using LubHub.Infrastructure.Services.TwitchApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace LubHub.Infrastructure.Extensions;

/// <summary>
/// Extension methods for registering Infrastructure layer services
/// </summary>
public static class InfrastructureServiceExtensions
{
    /// <summary>
    /// Registers all Infrastructure layer services into the DI container
    /// </summary>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ITwitchAuthService, TwitchAuthService>();
        services.AddScoped<IJwtService, JwtService>();

        var redisConnection = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Connection string 'Redis' not found");

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddScoped<IRedisService, RedisService>();

        return services;
    }
}