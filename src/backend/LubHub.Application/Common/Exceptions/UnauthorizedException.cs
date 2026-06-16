namespace LubHub.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when a user is not authorized to perform an action
/// </summary>
/// <param name="message">Description of the authorization failure</param>
public class UnauthorizedException(string message) : Exception(message);