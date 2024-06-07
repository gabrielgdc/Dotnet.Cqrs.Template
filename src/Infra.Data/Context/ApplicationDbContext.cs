using Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator _bus;
    private readonly DatabaseConfiguration _databaseConfiguration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, DatabaseConfiguration databaseConfiguration) : base(options)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _bus = mediator;
        _databaseConfiguration = databaseConfiguration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseOracle(_databaseConfiguration.ConnectionString, o => o.UseOracleSQLCompatibility("11"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_databaseConfiguration.DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Saves tracked entities to the database, dispatching domain events prior to saving.
    /// </summary>
    /// <param name="cancellationToken" type="System.Threading.CancellationToken">A cancellation token to signal cancellation requests.</param>
    /// <returns>True if the save operation was successful.</returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _bus.DispatchDomainEventsAsync(this, cancellationToken);

        await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}