namespace Infra.CrossCutting.Environments.Configurations;

/// <summary>
/// Represents the database configuration settings loaded from the application settings (environment variables).
/// </summary>
public class DatabaseConfiguration
{
    /// <summary>
    /// The connection string to be used for connecting to the database.
    /// </summary>
    public string ConnectionString { get; init; }

    /// <summary>
    /// The default schema to be used within the database.
    /// </summary>
    public string DefaultSchema { get; init; }
}