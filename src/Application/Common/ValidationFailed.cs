using System.Collections.Generic;
using FluentValidation.Results;

namespace Application.Common;

public record ValidationFailed(IEnumerable<ValidationFailure> ValidationFailures);
