using System.Collections.Generic;
using FluentValidation.Results;

namespace Application.Common;

/// <summary>
/// Represents a failed validation result containing a collection of validation failures.
/// </summary>
public record ValidationFailed(IEnumerable<ValidationFailure> ValidationFailures);
