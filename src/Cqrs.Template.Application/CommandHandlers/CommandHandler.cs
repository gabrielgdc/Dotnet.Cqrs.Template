using System.Threading;
using MediatR;
using System.Threading.Tasks;
using Cqrs.Template.Domain.SeedWork;
using Cqrs.Template.Domain.Exceptions;
using Cqrs.Template.Application.Commands;

namespace Cqrs.Template.Application.CommandHandlers;

public abstract class CommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : Command<TResponse>
{
    private readonly IUnitOfWork _uow;
    protected readonly IMediator Bus;
    private readonly ExceptionNotificationHandler _notifications;

    protected CommandHandler(IUnitOfWork uow, IMediator bus, INotificationHandler<ExceptionNotification> notifications)
    {
        _uow = uow;
        Bus = bus;
        _notifications = (ExceptionNotificationHandler)notifications;
    }

    public async Task<bool> CommitAsync()
    {
        if (_notifications.HasNotifications()) return false;
        if (await _uow.CommitAsync()) return true;

        await Bus.Publish(new ExceptionNotification("002", "We had a problem during saving your data.", ExceptionType.Server));

        return false;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
