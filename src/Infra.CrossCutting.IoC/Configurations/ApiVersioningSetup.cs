using System;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configurations;

/// <summary>
/// Contains extension methods for configuring API versioning within an ASP.NET Core application.
/// </summary>
public static class ApiVersioningSetup
{
    /// <summary>
    /// Extends the given IServiceCollection with API versioning support, configuring URL formatting and API explorer configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to extend.</param>
    /// <exception cref="ArgumentNullException">Thrown if the services parameter is null.</exception>
    public static void AddEndpointVersioning(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddApiVersioning();
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}
