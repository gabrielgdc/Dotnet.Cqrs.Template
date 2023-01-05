using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Template.Application.Queries;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.Data.Sqlite;

namespace Cqrs.Template.Application.QueryHandlers;

public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>
{
    private readonly IDbConnection _dbConnection;

    protected QueryHandler(ApplicationConfiguration applicationConfiguration)
    {
        _dbConnection = new SqliteConnection(applicationConfiguration.ConnectionString);
    }

    protected IDbConnection GetDatabaseConnection()
    {
        if (_dbConnection.State == ConnectionState.Closed)
        {
            _dbConnection.Open();
        }

        return _dbConnection;
    }

    protected void CloseDatabaseConnection()
    {
        if (_dbConnection.State is ConnectionState.Open or ConnectionState.Broken)
        {
            _dbConnection.Close();
        }
    }

    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
