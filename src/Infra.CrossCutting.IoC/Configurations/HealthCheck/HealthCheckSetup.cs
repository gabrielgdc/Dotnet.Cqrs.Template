using System;
using HealthChecks.UI.Client;
using Infra.CrossCutting.Environments.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infra.CrossCutting.IoC.Configurations.HealthCheck;

public static class HealthCheckSetup
{
    /// <summary>
    /// Configures health checks within the service collection.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">
    /// The service collection to add health checks to.
    /// </param>
    public static void AddHealthCheck(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("SELF CHECK API", () => HealthCheckResult.Healthy("HealthCheck Working For Cqrs.Template"));

        hcBuilder.AddCheck<RequiredSectionsHealthCheck<DatabaseConfiguration>>(nameof(DatabaseConfiguration));

        var applicationConfiguration = serviceProvider.GetRequiredService<DatabaseConfiguration>();
        hcBuilder.AddOracle(applicationConfiguration.ConnectionString, name: "ORACLE HEALTHCHECK", timeout: TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// Maps health check endpoints to specific paths within the application.
    /// </summary>
    /// <param name="endpoints" type="Microsoft.AspNetCore.Builder.IEndpointRouteBuilder">
    /// The IEndpointRouteBuilder instance used for endpoint configuration.
    /// </param>
    public static void MapHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/_health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecks("/_live", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("SELF")
        });
    }
}
