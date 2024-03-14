namespace Domain.SeedWork;

public interface IRepository<in TEntity> where TEntity : IAggregateRoot
{
    /// <summary>
    /// Adds a new entity of type TEntity to the underlying DbSet.
    /// </summary>
    /// <param name="obj">The entity object to be added.</param>
    void Add(TEntity obj);
}
