using Application.Commands;
using Infra.CrossCutting.Environments.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC;

/// <summary>
/// A static class responsible for registering services within the application's dependency injection container.
/// </summary>
public static class NativeInjectorBootstrapper
{
    /// <summary>
    /// Registers essential services with the service collection.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">The service collection to register services with.</param>
    /// <param name="configuration" type="Microsoft.Extensions.Configuration.IConfiguration">The application's configuration.</param>
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterEnvironments(services, configuration);
    }

    /// <summary>
    /// Configures and registers the Mediator pattern for handling commands and notifications.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">The service collection to register MediatR services with.</param>
    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(CommandHandler<,>));
            c.Lifetime = ServiceLifetime.Scoped;
        });
    }

    /// <summary>
    /// Registers application environment variables.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">The service collection to register services with.</param>
    /// <param name="configuration" type="Microsoft.Extensions.Configuration.IConfiguration">The applications configurations instances.</param>
    private static void RegisterEnvironments(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>());
    }
}