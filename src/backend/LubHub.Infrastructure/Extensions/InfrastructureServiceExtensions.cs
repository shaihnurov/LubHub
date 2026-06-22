using LubHub.Application.Common.Interfaces;
using LubHub.Infrastructure.Consumers;
using LubHub.Infrastructure.Services.Auth;
using LubHub.Infrastructure.Services.EventBus;
using LubHub.Infrastructure.Services.Redis;
using LubHub.Infrastructure.Services.SignalR;
using LubHub.Infrastructure.Services.TwitchApi;
using MassTransit;
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
        services.AddScoped<IRaffleHubService, RaffleHubService>();

        var redisConnection = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Connection string 'Redis' not found");

        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
        services.AddScoped<IRedisService, RedisService>();

        var rabbitHost = configuration["RabbitMQ:Host"]
            ?? throw new InvalidOperationException("RabbitMQ:Host not found");
        var rabbitUser = configuration["RabbitMQ:Username"]
            ?? throw new InvalidOperationException("RabbitMQ:Username not found");
        var rabbitPass = configuration["RabbitMQ:Password"]
            ?? throw new InvalidOperationException("RabbitMQ:Password not found");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<JoinRaffleConsumer>();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(rabbitHost, h =>
                {
                    h.Username(rabbitUser);
                    h.Password(rabbitPass);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddScoped<IEventBus, EventBusService>();

        return services;
    }
}