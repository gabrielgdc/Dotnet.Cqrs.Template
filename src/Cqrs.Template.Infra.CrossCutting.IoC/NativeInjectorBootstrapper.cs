using System;
using Cqrs.Template.Application.Behaviors;
using Cqrs.Template.Domain.Exceptions;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC;

public static class NativeInjectorBootstrapper
{
    public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        RegisterData(services);
        RegisterMediator(services);
        RegisterEnvironments(services, configuration);
    }

    private static void RegisterData(IServiceCollection services)
    {
        services.AddMemoryCache();
        // here goes your repository injection
        // sample: services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void RegisterMediator(IServiceCollection services)
    {
        const string applicationAssemblyName = "Cqrs.Template.Application"; // use your project name
        var assembly = AppDomain.CurrentDomain.Load(applicationAssemblyName);

        AssemblyScanner
            .FindValidatorsInAssembly(assembly)
            .ForEach(result => services.AddScoped(result.InterfaceType, result.ValidatorType));

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PipelineBehavior<,>));
        services.AddScoped<INotificationHandler<ExceptionNotification>, ExceptionNotificationHandler>();
    }

    private static void RegisterEnvironments(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>());
    }
}
