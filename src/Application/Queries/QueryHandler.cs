using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using Oracle.ManagedDataAccess.Client;

namespace Application.Queries;

public abstract class QueryHandler<TQuery, TResponse>(DatabaseConfiguration databaseConfiguration, ILogger logger) : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
    where TResponse : IOneOf
{
    protected readonly DatabaseConfiguration DatabaseConfiguration = databaseConfiguration;
    protected readonly ILogger Logger = logger;

    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);

    protected IDbConnection CreateDatabaseConnection()
    {
        Logger.LogInformation("Initializing database connection...");
        return new OracleConnection(DatabaseConfiguration.ConnectionString);
    }
}
