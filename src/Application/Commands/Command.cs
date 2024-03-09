using FluentValidation.Results;
using MediatR;
using OneOf;

namespace Application.Commands;

public abstract class Command<TResponse> : IRequest<TResponse> where TResponse : IOneOf
{
    protected ValidationResult ValidationResult { get; set; }

    public ValidationResult GetValidationResult()
    {
        return ValidationResult;
    }

    public abstract bool IsValid();
}
