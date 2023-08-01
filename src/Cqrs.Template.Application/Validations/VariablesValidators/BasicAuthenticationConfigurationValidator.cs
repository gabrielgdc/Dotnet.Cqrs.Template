using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using FluentValidation;

namespace Cqrs.Template.Application.Validations.VariablesValidators;

public class BasicAuthenticationConfigurationValidator : Validator<BasicAuthenticationConfiguration>
{
    public BasicAuthenticationConfigurationValidator()
    {
        RuleFor(c => c.Username)
            .NotEmpty()
            .NotNull();

        RuleFor(c => c.Password)
            .MinimumLength(8)
            .NotEmpty()
            .NotNull();
    }
}
