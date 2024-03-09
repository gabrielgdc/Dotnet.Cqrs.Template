namespace Domain.SeedWork;

public interface IRepository<in TEntity> where TEntity : IAggregateRoot
{
    /// <summary>
    /// Creates a problem details model from a ValidationFailed object.
    /// </summary>
    /// <param name="validationFailed">A validation failed containing all failed validations</param>
    void Add(TEntity obj);
}
