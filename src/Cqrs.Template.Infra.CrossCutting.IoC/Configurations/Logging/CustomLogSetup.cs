using System;
using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Microsoft.AspNetCore.Builder;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging;

public static class CustomLogSetup
{
    public static void AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));

        const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        var loggerConfigurations = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails();

        var environment = configuration.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>().Environment;

        if (string.Equals(environment, "Development", StringComparison.InvariantCultureIgnoreCase))
        {
            loggerConfigurations.WriteTo.Async(logger => logger.Console(outputTemplate: outputTemplate));
        }
        else
        {
            // TODO: Add custom sink if needed
        }

        Log.Logger = loggerConfigurations.CreateLogger();
    }

    public static void UseLoggingMiddlewares(this IApplicationBuilder app)
    {
        // app.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
    }
}
