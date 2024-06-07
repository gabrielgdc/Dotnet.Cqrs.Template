using FluentValidation.Results;
using MediatR;
using OneOf;

namespace Application.Commands;

/// <summary>
/// Represents a base class for defining commands within the MediatR library.
/// </summary>
/// <typeparam name="TResponse">The type of response expected from the command handler. 
/// This type must implement the `IOneOf` interface.</typeparam>
/// <remarks>
/// This abstract class provides a foundation for building MediatR commands in your application. 
/// It leverages the `IOneOf` interface to represent a variety of possible command responses, 
/// including success, failure with validation errors, or other potential outcomes.
/// 
/// Subclasses should inherit from this class and implement the `IsValid` method to define command validation logic.
/// They can also define specific properties and behavior relevant to their command functionality.
/// </remarks>
public abstract class Command<TResponse> : IRequest<TResponse> where TResponse : IOneOf
{
    /// <summary>
    /// The validation result object containing any validation errors encountered during command execution.
    /// </summary>
    protected ValidationResult ValidationResult { get; set; }

    /// <summary>
    /// Retrieves the validation results for the command.
    /// </summary>
    /// <returns>A FluentValidation.Results.ValidationResult object containing validation errors (if any).</returns>
    public ValidationResult GetValidationResult()
    {
        return ValidationResult;
    }

    /// <summary>
    /// Performs validation for the command and populates the internal ValidationResult property.
    /// </summary>
    /// <returns>True if the command is valid, false otherwise.</returns>
    public abstract bool IsValid();
}