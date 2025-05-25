namespace Infra.CrossCutting.Environments.Configurations;

/// <summary>
/// Represents the application logging behavior configuration
/// </summary>
public record LoggingConfiguration
{
    public bool LogsOnConsole { get; set; }
}