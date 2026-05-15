using FluentValidation;
using MediatR;
using ApplicationValidationException = Lamie.Application.Common.Exceptions.ValidationException;

namespace Lamie.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that runs all FluentValidation validators registered for the request type.
/// Throws our application <see cref="ApplicationValidationException"/> so the global middleware turns it into a 400.
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .GroupBy(f => string.IsNullOrWhiteSpace(f.PropertyName) ? "_" : char.ToLowerInvariant(f.PropertyName[0]) + f.PropertyName[1..])
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).Distinct().ToArray());

        if (failures.Count > 0)
        {
            throw new ApplicationValidationException(failures);
        }

        return await next();
    }
}
