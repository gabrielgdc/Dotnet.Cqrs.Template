using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.SeedWork;
using Infra.Data.Context;
using MediatR;

namespace Infra.Data;

public static class MediatorExtension
{
    /// <summary>
    /// Dispatches domain events associated with tracked entities asynchronously.
    /// </summary>
    /// <remarks>
    /// This method will execute, handle and delete all the domain events after it successfully handled all the events 
    /// </remarks>
    /// <param name="mediator" type="IMediator">The mediator instance for publishing events.</param>
    /// <param name="dbContext" type="ApplicationDbContext">The application's DbContext.</param>
    /// <param name="cancellationToken" type="System.Threading.CancellationToken">A cancellation token to signal cancellation requests.</param>
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, ApplicationDbContext dbContext, CancellationToken cancellationToken)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvent());
    }
}
