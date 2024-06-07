using Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using OneOf;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Queries;

/// <summary>
/// Represents a base class for defining query handlers within the MediatR library, providing access to common dependencies and database interaction.
/// </summary>
/// <typeparam name="TQuery">The type of query that this handler is responsible for processing. 
/// This type must inherit from the `Query` base class.</typeparam>
/// <typeparam name="TResponse">The type of response that the query handler can produce. 
/// This type must implement the `IOneOf` interface.</typeparam>
/// <remarks>
/// This abstract class establishes a foundation for building MediatR query handlers. 
/// It injects and provides access to essential dependencies like database configuration, logger, and string localizer.
/// It also offers a helper method to create database connections and allows subclasses to implement specific logic for handling queries of type `TQuery`.
/// 
/// Subclasses should inherit from this class and implement the `Handle` method to define the processing logic for the associated query type.
/// They can leverage the provided dependencies for database access, logging, and localization within their implementation.
/// </remarks>
public abstract class QueryHandler<TQuery, TResponse>(
    DatabaseConfiguration databaseConfiguration,
    ILogger logger,
    IStringLocalizer<QueryHandler<TQuery, TResponse>> stringLocalizer
) : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
    where TResponse : IOneOf
{
    /// <summary>
    /// A reference to the injected database configuration object containing connection details.
    /// </summary>
    protected readonly DatabaseConfiguration DatabaseConfiguration = databaseConfiguration;

    /// <summary>
    /// A reference to the injected string localizer instance for retrieving localized messages within the query handler.
    /// </summary>
    protected readonly IStringLocalizer<QueryHandler<TQuery, TResponse>> StringLocalizer = stringLocalizer;

    /// <summary>
    /// A reference to the injected logger instance for logging messages within the query handler.
    /// </summary>
    protected readonly ILogger Logger = logger;

    /// <summary>
    /// Handles the execution of the specified query.
    /// </summary>
    /// <param name="request">The query object to be processed.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to signal cancellation of the operation.</param>
    /// <returns>A Task representing the asynchronous operation that produces the query response of type TResponse.</returns>
    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new database connection using the injected database configuration.
    /// </summary>
    /// <returns>An IDbConnection object representing the established database connection.</returns>
    protected IDbConnection CreateDatabaseConnection()
    {
        Logger.LogInformation("Initializing database connection...");
        return new OracleConnection(DatabaseConfiguration.ConnectionString);
    }
}