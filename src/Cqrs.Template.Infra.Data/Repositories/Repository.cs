using Cqrs.Template.Domain.SeedWork;
using Cqrs.Template.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Cqrs.Template.Infra.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    protected readonly ApplicationDbContext ApplicationDbContext;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(ApplicationDbContext applicationDbContext)
    {
        ApplicationDbContext = applicationDbContext;
        DbSet = applicationDbContext.Set<TEntity>();
    }

    public void Add(TEntity obj)
    {
        ApplicationDbContext.Add(obj);
    }
}
