using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.CrossCutting.IoC.Configurations.HealthCheck;

internal class RequiredVariablesHealthCheck : IHealthCheck
{
    /// <summary>
    /// Performs a health check for an environment variable.
    /// </summary>
    /// <param name="context" type="Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckContext">The context containing information about the health check.</param>
    /// <param name="cancellationToken" type="System.Threading.CancellationToken">A cancellation token to signal cancellation requests (optional).</param>
    /// <returns>A task that returns the health check result for the environment variable.</returns>
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var variable = Environment.GetEnvironmentVariable(context.Registration.Name);

        return Task.FromResult(
            !string.IsNullOrWhiteSpace(variable)
                ? HealthCheckResult.Healthy("Variable mapped")
                : HealthCheckResult.Unhealthy($"Variable not mapped: {context.Registration.Name}")
        );
    }
}