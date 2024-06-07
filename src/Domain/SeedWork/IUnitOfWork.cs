using System.Threading.Tasks;

namespace Domain.SeedWork;

public interface IUnitOfWork
{
    /// <summary>
    /// Commits changes to the database by calling SaveEntitiesAsync on the applicationDbContext.
    /// </summary>
    /// <returns>True if the commit operation was successful (i.e., SaveEntitiesAsync returned true).</returns>
    Task<bool> CommitAsync();
}