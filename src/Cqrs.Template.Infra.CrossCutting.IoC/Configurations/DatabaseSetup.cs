using System;
using Cqrs.Template.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations;

public static class DatabaseSetup
{
	public static void AddDatabaseSetup(this IServiceCollection services)
	{
        ArgumentNullException.ThrowIfNull(services);

		services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);
	}
}
