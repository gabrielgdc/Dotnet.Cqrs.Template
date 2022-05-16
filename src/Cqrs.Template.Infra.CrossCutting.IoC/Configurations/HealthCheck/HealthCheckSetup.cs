using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.HealthCheck;

public static class HealthCheckSetup
{
    public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        hcBuilder.AddCheck("Self Check API", () => HealthCheckResult.Healthy("HealthCheck Working"));
        // ADD OTHER CHECKS HERE

        hcBuilder.AddCheck<RequiredSectionsHealthCheck<ApplicationConfiguration>>(nameof(ApplicationConfiguration));
    }

    public static void MapHealthCheck(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
    }
}
