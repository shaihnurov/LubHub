namespace LubHub.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested entity is not found
    /// </summary>
    /// <param name="entityName">Name of the entity that was not found</param>
    /// <param name="key">Key used to look up the entity</param>
    public class NotFoundException(string entityName, object key) : Exception($"{entityName} with key '{key}' was not found");
}