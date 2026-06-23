namespace LubHub.API.Extensions;

/// <summary>
/// Extension methods for configuring CORS
/// </summary>
public static class CorsExtensions
{
    private const string CorsPolicyName = "LubHubCors";

    /// <summary>
    /// Registers CORS with allowed origins from configuration.
    /// AllowCredentials is required for SignalR WebSocket authentication.
    /// </summary>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Applies the configured CORS policy. Must be called before UseAuthentication and UseAuthorization.
    /// </summary>
    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app)
        => app.UseCors(CorsPolicyName);
}