using System;
using Domain.SeedWork;
using Infra.Data.Context;
using Infra.Data.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configurations;

public static class DatabaseSetup
{
    public static void AddDatabaseSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
