using System;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;

public static class CustomAuthenticationSetup
{
    public static void AddCustomAuthentication(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddAuthentication()
            .AddScheme<BasicAuthenticationSchemeOptions, BasicAuthenticationScheme>(CustomAuthenticationSchemes.Basic, null);
    }
}
