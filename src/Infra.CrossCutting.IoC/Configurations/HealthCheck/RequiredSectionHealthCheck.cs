using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infra.CrossCutting.IoC.Configurations.HealthCheck;

internal class RequiredSectionsHealthCheck<T>(IConfiguration configuration) : IHealthCheck
{
    /// <summary>
    /// Performs a health check for a specific configuration section.
    /// </summary>
    /// <param name="context" type="Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext">The context containing information about the health check.</param>
    /// <param name="cancellationToken" type="System.Threading.CancellationToken">A cancellation token to signal cancellation requests (optional).</param>
    /// <returns>A task that returns the health check result for the configuration section.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var section = configuration.GetSection(context.Registration.Name).Get<T>();

        if (section is null)
            return Task.FromResult(HealthCheckResult.Unhealthy($"Section {context.Registration.Name} not mapped"));

        var properties = section.GetType().GetProperties();

        var missingProperties = properties
            .Where(x => x.GetValue(section) is null)
            .Select(x => x.Name)
            .ToList();

        var mappedProperties = properties
            .Where(x => x.GetValue(section) is not null)
            .Select(x => x.Name)
            .ToList();

        IReadOnlyDictionary<string, object> data = new Dictionary<string, object>
        {
            { "MissingProperties", string.Join(" | ", missingProperties) },
            { "MappedProperties", string.Join(" | ", mappedProperties) },
        };

        return Task.FromResult(
            !missingProperties.Any()
                ? HealthCheckResult.Healthy($"Section {context.Registration.Name} mapped", data: data)
                : HealthCheckResult.Unhealthy(
                    $"Section {context.Registration.Name} not properly mapped, missing properties", data: data)
        );
    }
}
