using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace Infra.CrossCutting.Ioc.Configurations.Swagger;

public static class SwaggerSetup
{
    /// <summary>
    /// Configures Swagger documentation generation for the application.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">
    /// The service collection to add Swagger services to.
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if the `services` parameter is null.</exception>
    public static void AddSwaggerSetup(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = string.Join(" ", "Cqrs.Template".Split(".")),
                Description = "Some description"
            });

            s.EnableAnnotations();
        });

    }

    /// <summary>
    /// Maps Swagger endpoints for multiple API versions within the application.
    /// </summary>
    /// <param name="app" type="Microsoft.AspNetCore.Builder.IApplicationBuilder">The application builder instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if the `app` parameter is null.</exception>
    public static void MapSwagger(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);

        var versionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var description in versionProvider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
    }
}