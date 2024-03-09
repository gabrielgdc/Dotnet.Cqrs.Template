using System;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configurations;

public static class ApiVersioningSetup
{
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
