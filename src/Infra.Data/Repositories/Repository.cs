using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Repositories;

/// <summary>
/// Provides a base class for implementing repositories that manage entities inheriting from IAggregateRoot.
/// </summary>
/// <typeparam name="TEntity">The type of entity the repository manages.</typeparam>
/// <remarks>
/// This abstract class provides basic CRUD (Create, Read, Update, Delete) operations for entities 
/// that implement the `IAggregateRoot` interface. It expects a `DbContext` instance to be injected 
/// in the constructor for accessing the database. Subclasses can implement additional methods 
/// specific to their entity type.
/// </remarks>
public abstract class Repository<TEntity>(DbContext dbContext) : IRepository<TEntity> where TEntity : class, IAggregateRoot
{
    /// <summary>
    /// The internal DbSet instance for the entity type managed by the repository.
    /// </summary>
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();
    
    public void Add(TEntity obj)
    {
        DbSet.Add(obj);
    }
}
