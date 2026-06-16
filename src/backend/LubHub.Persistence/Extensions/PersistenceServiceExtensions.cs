using LubHub.Application.Common.Interfaces;
using LubHub.Persistence.Context;
using LubHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LubHub.Persistence.Extensions;

/// <summary>
/// Extension methods for registering Persistence layer services
/// </summary>
public static class PersistenceServiceExtensions
{
    /// <summary>
    /// Registers all Persistence layer services into the DI container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The updated service collection</returns>
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IRaffleRepository, RaffleRepository>();
        services.AddScoped<IStreamerRepository, StreamerRepository>();
        services.AddScoped<IParticipantRepository, ParticipantRepository>();

        return services;
    }
}