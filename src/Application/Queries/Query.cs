using FluentValidation.Results;
using MediatR;
using OneOf;

namespace Application.Queries;

public abstract class Query<TResponse> : IRequest<TResponse> where TResponse : IOneOf
{
    public ValidationResult ValidationResult { get; protected set; }

    public abstract bool IsValid();
}
