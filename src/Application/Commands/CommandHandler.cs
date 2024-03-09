using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Application.Commands;

public abstract class CommandHandler<TRequest, TResponse>(IUnitOfWork uow, ILogger logger) : IRequestHandler<TRequest, TResponse>
    where TRequest : Command<TResponse>
    where TResponse : IOneOf
{
    protected readonly ILogger Logger = logger;

    public async Task<bool> CommitAsync()
    {
        if (await uow.CommitAsync()) return true;

        Logger.LogCritical("Problem on saving changes in database");

        return false;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
