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
    /// Dispatch and handle all domain events triggered in a scoped request.
    /// </summary>
    /// <remarks>
    /// This method will execute, handle and delete all the domain events after it successfully handled all the events 
    /// </remarks>
    /// <param name="mediator">An instance of mediator to be extended</param>
    /// <param name="dbContext">The database context</param>
    /// <param name="cancellationToken">A cancellation token</param>
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

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvent());
    }
}
