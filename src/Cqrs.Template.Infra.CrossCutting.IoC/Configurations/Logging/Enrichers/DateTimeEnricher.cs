using System;
using Serilog.Core;
using Serilog.Events;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Logging.Enrichers;

public class DateTimeEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var property = propertyFactory.CreateProperty("DateTime", new ScalarValue(DateTime.UtcNow));
        logEvent.AddPropertyIfAbsent(property);
    }
}
