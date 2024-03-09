using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Application.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var commandName = request.GetType().Name;
        // test
        // test
        return await next();

        logger.LogInformation("Validating command {CommandName}", commandName);

        var failures = validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        logger.LogInformation("Command {CommandName} validate with {ValidationErrorsQuantity} validation errors", commandName, failures.Count);

        if (!failures.Any()) return await next();

        logger.LogWarning("Validation errors - {CommandName} - Command: {Command} - Errors: {ValidationErrors}", commandName, request, failures);

        return default;
    }
}
