using Infra.CrossCutting.Environments.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Reflection;

namespace Infra.CrossCutting.Ioc.Configurations.Logging;

public static class LoggingSetup
{
    public static void AddLoggingSetup(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        var loggerConfigurations = new LoggerConfiguration()
            .ConfigureMinimumLevel()
            .ConfigureFilters()
            .ConfigureEnrichers();

        var environment = configuration.GetSection(nameof(LoggingConfiguration)).Get<LoggingConfiguration>();

        if (environment.LogsOnConsole)
        {
            loggerConfigurations.WriteTo.Console(outputTemplate: outputTemplate);
        }

        // Add custom sink if needed

        Log.Logger = loggerConfigurations.CreateLogger();
    }
    
    private static LoggerConfiguration ConfigureEnrichers(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            
            .Enrich.WithProperty("SystemName", new ScalarValue(Assembly.GetEntryAssembly()?.GetName().Name))
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationId();
    }

    private static LoggerConfiguration ConfigureMinimumLevel(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    }

    private static LoggerConfiguration ConfigureFilters(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration;
    }
}