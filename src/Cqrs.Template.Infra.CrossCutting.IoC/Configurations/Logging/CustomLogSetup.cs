using System;
using Serilog;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging;

public static class CustomLogSetup
{
    public static void AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        var loggerConfigurations = new LoggerConfiguration()
            .ConfigureMinimumLevel()
            .ConfigureFilters()
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails();

        var environment = configuration.GetSection(nameof(ApplicationConfiguration)).Get<ApplicationConfiguration>().Environment;

        if ( string.Equals(environment, "Development", StringComparison.InvariantCultureIgnoreCase) )
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
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHeader", httpContext.Request.Headers);
                diagnosticContext.Set("ResponseHeader", httpContext.Response.Headers);
            };
        });
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
            .Filter.ByExcluding("if StatusCode = 200 then RequestPath like '/hc' or '/health%' else ''")
            .Filter.ByExcluding("RequestPath like '%liveness%'")
            .Filter.ByExcluding("RequestPath like '%swagger%'")
            .Filter.ByExcluding("RequestPath like '%swagger%'")
            .Filter.ByExcluding("QueryString like 'password' or 'psw' or 'senha'");
    }
}
