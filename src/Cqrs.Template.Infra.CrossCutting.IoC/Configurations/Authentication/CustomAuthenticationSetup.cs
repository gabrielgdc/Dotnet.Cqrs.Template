using System;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;

public static class CustomAuthenticationSetup
{
    public static void AddCustomAuthentication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddAuthentication()
            .AddScheme<BasicAuthenticationSchemeOptions, BasicAuthenticationScheme>(CustomAuthenticationSchemes.Basic, null);
    }
}
