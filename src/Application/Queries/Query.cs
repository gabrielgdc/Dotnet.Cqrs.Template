using FluentValidation.Results;
using MediatR;
using OneOf;

namespace Application.Queries;

/// <summary>
/// Represents a base class for defining queries within the MediatR library, incorporating validation support.
/// </summary>
/// <typeparam name="TResponse">The type of response that the query can produce. 
/// This type must implement the `IOneOf` interface.</typeparam>
/// <remarks>
/// This abstract class provides a foundation for implementing MediatR queries. 
/// It inherits from the `IRequest` interface, enabling participation in the MediatR request pattern.
/// Additionally, it introduces validation functionality through the `ValidationResult` property and the abstract `IsValid` method.
/// 
/// Subclasses should inherit from this class and implement the following:
/// - Specific logic for retrieving or manipulating data within the `Execute` method (not defined in this base class).
/// - Validation rules using FluentValidation or other validation libraries within the `IsValid` method.
/// </remarks>
public abstract class Query<TResponse> : IRequest<TResponse> where TResponse : IOneOf
{
    /// <summary>
    /// Stores the validation results associated with the query execution.
    /// </summary>
    public ValidationResult ValidationResult { get; protected set; }

    /// <summary>
    /// An abstract method that subclasses must implement to define the validation logic for the query.
    /// This method should perform any necessary validations and store the results in the `ValidationResult` property.
    /// </summary>
    /// <returns>True if the query is valid, false otherwise.</returns>
    public abstract bool IsValid();
}