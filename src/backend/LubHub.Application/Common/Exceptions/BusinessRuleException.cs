namespace LubHub.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    /// <param name="message">Description of the violated rule</param>
    public class BusinessRuleException(string message) : Exception(message);
}