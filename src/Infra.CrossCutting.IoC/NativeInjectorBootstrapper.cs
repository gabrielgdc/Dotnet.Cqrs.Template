using Application.Behaviors;
using Application.Commands;
using Domain.Exceptions;
using Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC;

public static class NativeInjectorBootstrapper
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterMediator(services);
        RegisterEnvironments(services, configuration);
    }

    private static void RegisterMediator(IServiceCollection services)
    {
        services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssemblyContaining(typeof(CommandHandler<,>));
            c.AddOpenBehavior(typeof(ValidatorBehavior<,>));
            c.Lifetime = ServiceLifetime.Scoped;
        });

        services.AddScoped<INotificationHandler<ExceptionNotification>, ExceptionNotificationHandler>();
    }

    private static void RegisterEnvironments(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(DatabaseConfiguration)).Get<DatabaseConfiguration>());
    }
}
