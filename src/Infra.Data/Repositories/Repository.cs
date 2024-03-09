using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

public abstract class Repository<TEntity>(DbContext dbContext) : IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public void Add(TEntity obj)
    {
        DbSet.Add(obj);
    }
}
