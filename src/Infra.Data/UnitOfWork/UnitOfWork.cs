using Domain.SeedWork;
using Infra.Data.Context;
using System.Threading.Tasks;

namespace Infra.Data.UnitOfWork;

public class UnitOfWork(ApplicationDbContext applicationDbContext) : IUnitOfWork
{
    public async Task<bool> CommitAsync()
    {
        return await applicationDbContext.SaveEntitiesAsync();
    }
}