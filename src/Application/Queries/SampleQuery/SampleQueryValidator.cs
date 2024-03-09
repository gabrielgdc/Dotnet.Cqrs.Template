using FluentValidation;

namespace Application.Queries.SampleQuery;

public class SampleQueryValidator : AbstractValidator<SampleQuery>
{
    public SampleQueryValidator()
    {
        RuleFor(q => q.ForceClientException)
            .NotNull();
    }
}
