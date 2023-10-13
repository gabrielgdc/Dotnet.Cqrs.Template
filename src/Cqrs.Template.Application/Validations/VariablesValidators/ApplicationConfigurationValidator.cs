using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using FluentValidation;

namespace Cqrs.Template.Application.Validations.VariablesValidators;

public class ApplicationConfigurationValidator : Validator<ApplicationConfiguration>
{
    public ApplicationConfigurationValidator()
    {
        RuleFor(c => c.Environment)
            .Must(x => x.Equals("Development") || x.Equals("Homolog") || x.Equals("Production"))
            .WithMessage("must be 'Development', 'Homolog' or 'Production");

        RuleFor(c => c.ConnectionString)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.Schema)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.GlobalErrorCode)
            .NotNull()
            .NotEmpty();

        RuleFor(c => c.GlobalErrorMessage)
            .NotNull()
            .NotEmpty();
    }
}
