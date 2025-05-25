namespace Infra.CrossCutting.Environments.Configurations;

/// <summary>
/// Represents the application logging behavior configuration
/// </summary>
public record LoggingConfiguration
{
    /// <summary>
    /// Toggles logs on console
    /// </summary>
    public bool LogsOnConsole { get; init; }
}