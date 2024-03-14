using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configurations;

public static class DependencyInjectionSetup
{
    /// <summary>
    /// Adds essential services to the service collection, leveraging the NativeInjectorBootstrapper for registration.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">The service collection to add services to.</param>
    /// <param name="configuration" type="Microsoft.Extensions.Configuration.IConfiguration">The application's configuration.</param>
    /// <exception cref="ArgumentNullException">Thrown if the `services` parameter is null.</exception>
    public static void AddDependencyInjectionSetup(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        NativeInjectorBootstrapper.RegisterServices(services, configuration);
    }
}
