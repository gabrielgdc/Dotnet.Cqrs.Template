using System;
using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging.Enrichers;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging;

public static class CustomLogSetup
{
    public static void AddCustomLogging(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        var loggerConfigurations = new LoggerConfiguration()
            .ConfigureMinimumLevel()
            .ConfigureFilters()
            .ConfigureEnrichers(serviceName);

        var environment = configuration.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>();

        if (environment.LogsOnConsole)
        {
            loggerConfigurations.WriteTo.Async(logger => logger.Console(outputTemplate: outputTemplate));
        }

        // Add custom sink if needed

        Log.Logger = loggerConfigurations.CreateLogger();
    }

    public static void UseLoggingMiddlewares(this IApplicationBuilder app)
    {
        // app.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHeader", httpContext.Request.Headers);
                diagnosticContext.Set("ResponseHeader", httpContext.Response.Headers);
            };
        });
    }

    private static LoggerConfiguration ConfigureEnrichers(this LoggerConfiguration loggerConfiguration, string serviceName)
    {
        return loggerConfiguration
            .Enrich.With<DateTimeEnricher>()
            .Enrich.WithProperty("ServiceName", new ScalarValue(serviceName))
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails();
    }

    private static LoggerConfiguration ConfigureMinimumLevel(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    }

    private static LoggerConfiguration ConfigureFilters(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .Filter.ByExcluding("if StatusCode = 200 then RequestPath like '/_health' or '/_health%' else ''")
            .Filter.ByExcluding("RequestPath like '%live%'")
            .Filter.ByExcluding("RequestPath like '%swagger%'")
            .Filter.ByExcluding("RequestPath like '%swagger%'")
            .Filter.ByExcluding("QueryString like 'password' or 'psw' or 'senha'");
    }
}
