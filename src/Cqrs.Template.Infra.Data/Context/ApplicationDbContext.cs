using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cqrs.Template.Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    private readonly IMediator _bus;
    private readonly ApplicationConfiguration _applicationConfiguration;

    public ApplicationDbContext(IOptions<ApplicationConfiguration> applicationConfiguration)
    {
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<ApplicationConfiguration> applicationConfiguration) : base(options)
    {
        _applicationConfiguration = applicationConfiguration.Value;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator, IOptions<ApplicationConfiguration> applicationConfiguration) : base(options)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _bus = mediator;
        _applicationConfiguration = applicationConfiguration.Value;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite(_applicationConfiguration.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema(_applicationConfiguration.Schema);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _bus.DispatchDomainEventsAsync(this);

        return await base.SaveChangesAsync(cancellationToken) > 0;
    }
}
