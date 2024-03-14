using System;
using Infra.CrossCutting.Environments.BaseConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Infra.CrossCutting.IoC.Configurations;

public static class HttpClientSetup
{
    /// <summary>
    /// Registers an HttpClient proxy for a specified interface and configuration type within the service collection.
    /// Leverages Refit for client generation and configures the base URL based on configuration settings.
    /// </summary>
    /// <param name="services" type="System.IServiceCollection">The service collection to register the HttpClient proxy.</param>
    /// <param name="configuration" type="Microsoft.Extensions.Configuration.IConfiguration">An instance of IConfiguration containing the configuration values.</param>
    /// <typeparam name="TConfiguration">The type representing the configuration for the HttpClient proxy. Inherits from BaseHttpClientProxyConfiguration.</typeparam>
    /// <typeparam name="TInterface">The interface representing the service contract for the HttpClient proxy.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if either `services` or `configuration` is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the configuration section corresponding to `TConfiguration` is not found.</exception>
    public static void AddHttpClientProxy<TInterface, TConfiguration>(this IServiceCollection services, IConfiguration configuration)
        where TConfiguration : BaseHttpClientProxyConfiguration
        where TInterface : class
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        var section = configuration.GetSection(typeof(TConfiguration).Name);

        if (!section.Exists()) throw new ArgumentNullException($"Section {typeof(TConfiguration).Name} not properly configured");

        var proxyConfiguration = section.Get<TConfiguration>();

        services.AddRefitClient<TInterface>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(proxyConfiguration.BaseUrl));
    }
}
