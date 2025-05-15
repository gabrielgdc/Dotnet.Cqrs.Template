using FluentValidation;

namespace Application.Features.SampleQuery;

/// <summary>
/// Represents a validator class for the SampleQuery, utilizing FluentValidation for defining validation rules.
/// </summary>
public class SampleQueryValidator : AbstractValidator<SampleQuery>
{
    public SampleQueryValidator()
    {
        RuleFor(q => q.ForceClientException)
            .NotNull();
    }
}