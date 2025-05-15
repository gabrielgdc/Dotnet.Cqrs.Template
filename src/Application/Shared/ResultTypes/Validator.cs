using FluentValidation;

namespace Application.Shared.ResultTypes;

/// <summary>
/// Represents a base class for defining validators using the FluentValidation library.
/// </summary>
/// <typeparam name="T">The type of object to be validated by this validator.</typeparam>
/// <remarks>
/// This abstract class inherits from `AbstractValidator` provided by FluentValidation. 
/// Subclasses should inherit from this class and implement specific validation rules for the target type (T). 
/// They can utilize FluentValidation's rich set of validation rules and methods to define their validation logic.
/// </remarks>
public abstract class Validator<T> : AbstractValidator<T>;