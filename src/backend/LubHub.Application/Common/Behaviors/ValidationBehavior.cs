using FluentValidation;
using MediatR;

namespace LubHub.Application.Common.Behaviors;

/// <summary>
/// Pipeline behavior that runs FluentValidation validators before handling a request
/// </summary>
/// <typeparam name="TRequest">Type of the request</typeparam>
/// <typeparam name="TResponse">Type of the response</typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    /// <summary>
    /// Validates the request before passing it to the next handler
    /// </summary>
    /// <param name="request">Incoming request</param>
    /// <param name="next">Next handler in the pipeline</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Response from the next handler</returns>
    /// <exception cref="ValidationException">Thrown if any validation rules fail</exception>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        if (!validators.Any())
            return await next(ct);

        var context = new ValidationContext<TRequest>(request);

        var failures = validators.Select(v => v.Validate(context)).SelectMany(e => e.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(ct);
    }
}