using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Template.Application.Queries;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Cqrs.Template.Application.QueryHandlers;

public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>
{
    protected readonly IDbConnection DbConnection;
    protected readonly IMediator Bus;

    protected QueryHandler(IOptions<ApplicationConfiguration> applicationConfiguration, IMediator bus)
    {
        DbConnection = new SqliteConnection(applicationConfiguration.Value.ConnectionString);
        Bus = bus;
    }

    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
