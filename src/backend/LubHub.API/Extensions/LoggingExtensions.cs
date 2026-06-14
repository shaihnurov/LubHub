using Serilog;

namespace LubHub.API.Extensions;

/// <summary>
/// Extension methods for configuring application logging
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures Serilog as the application logger with console and file sinks
    /// </summary>
    /// <param name="builder">The web application builder</param>
    /// <returns>The updated web application builder</returns>
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File(
                path: "logs/log.txt",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 7
            ).CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}