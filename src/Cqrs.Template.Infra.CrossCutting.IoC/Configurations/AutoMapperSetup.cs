using System;
using Cqrs.Template.Application.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations;

public static class AutoMapperSetup
{
	public static void AddAutoMapper(this IServiceCollection services)
	{
        ArgumentNullException.ThrowIfNull(services);

		services.AddAutoMapper(typeof(MappingProfile));
	}
}
