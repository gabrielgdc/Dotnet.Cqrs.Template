using System;
using Domain.SeedWork;
using Infra.Data.Context;
using Infra.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configurations;

public static class DatabaseSetup
{
    /// <summary>
    /// Adds database, repositories and unit of work services to the service collection.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">The service collection to add services to.</param>
    /// <exception cref="ArgumentNullException">Thrown if the `services` parameter is null.</exception>
    public static void AddDatabaseSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
