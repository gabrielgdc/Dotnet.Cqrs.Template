using System.Threading.Tasks;

namespace Domain.SeedWork;

public interface IUnitOfWork
{
    Task<bool> CommitAsync();
}
