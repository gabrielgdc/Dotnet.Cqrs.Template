using FluentValidation.Results;
using System.Collections.Generic;

namespace Application.Shared.ResultTypes;

/// <summary>
/// Represents a failed validation result containing a collection of validation failures.
/// </summary>
public record ValidationFailed(IEnumerable<ValidationFailure> ValidationFailures);