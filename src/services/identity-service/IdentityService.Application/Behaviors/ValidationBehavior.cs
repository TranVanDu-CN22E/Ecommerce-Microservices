using FluentValidation;
using IdentityService.Application.Abstractions.Messaging;
using IdentityService.Application.Common;
using MediatR;
namespace IdentityService.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, ct)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Distinct()
            .ToList();

        if (!failures.Any())
            return await next();

        var errors = failures
            .Select(f => new Error(
                f.ErrorCode,
                f.ErrorMessage))
            .ToList();

        return (TResponse)(object)Result.Failure(errors);
    }
}
