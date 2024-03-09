namespace Domain.SeedWork;

public interface IRepository<in TEntity> where TEntity : IAggregateRoot
{
    void Add(TEntity obj);
}
