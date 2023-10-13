using System.Linq;
using System.Threading.Tasks;
using Cqrs.Template.Domain.SeedWork;
using Cqrs.Template.Infra.Data.Context;
using MediatR;

namespace Cqrs.Template.Infra.Data;

public static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, ApplicationDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        domainEntities
            .ForEach(entity => entity.Entity.ClearDomainEvent());
    }
}
