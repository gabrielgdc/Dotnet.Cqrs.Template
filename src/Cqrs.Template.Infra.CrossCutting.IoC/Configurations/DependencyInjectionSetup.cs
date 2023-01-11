using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations;

public static class DependencyInjectionSetup
{
	public static void AddDependencyInjectionSetup(this IServiceCollection services, IConfiguration configuration)
	{
		if (services == null) throw new ArgumentNullException(nameof(services));

		NativeInjectorBootstrapper.RegisterServices(services, configuration);
	}
}
