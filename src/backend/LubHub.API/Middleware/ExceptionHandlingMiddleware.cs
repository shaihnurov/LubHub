using FluentValidation;
using LubHub.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LubHub.API.Middleware;

/// <summary>
/// Global exception handler that maps exceptions to appropriate HTTP responses
/// </summary>
public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IExceptionHandler
{
    /// <summary>
    /// Handles the exception and writes a ProblemDetails response
    /// </summary>
    /// <param name="context">Current HTTP context</param>
    /// <param name="exception">The exception that occurred</param>
    /// <returns>True if the exception was handled</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var (logLevel, status, title) = exception switch
        {
            NotFoundException => (LogLevel.Warning, StatusCodes.Status404NotFound, "Resource not found"),
            BusinessRuleException => (LogLevel.Warning, StatusCodes.Status409Conflict, "Business rule violated"),
            ValidationException => (LogLevel.Warning, StatusCodes.Status400BadRequest, "Validation error"),
            _ => (LogLevel.Error, StatusCodes.Status500InternalServerError, "Internal server error")
        };

        logger.Log(logLevel, exception, "Error processing request: {ExceptionMessage}", exception.Message);

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}