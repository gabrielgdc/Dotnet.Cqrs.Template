using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Infra.CrossCutting.Ioc.Configurations;

public static class CustomLocalizationSetup
{
    /// <summary>
    /// Configures localization services with specific settings for supported cultures and default culture.
    /// </summary>
    /// <param name="services" type="Microsoft.Extensions.DependencyInjection.IServiceCollection">
    /// The service collection to add localization services to.
    /// </param>
    public static void AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("pt-BR"),
                new CultureInfo("en-US")
            };

            options.DefaultRequestCulture = new RequestCulture(culture: "pt-BR");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
    }

    /// <summary>
    /// Applies custom localization settings to the application pipeline.
    /// </summary>
    /// <param name="app" type="Microsoft.AspNetCore.Builder.IApplicationBuilder">
    /// The current IApplicationBuilder instance representing the application pipeline.
    /// </param>
    /// <remarks>
    /// This extension method retrieves the configured RequestLocalizationOptions from the application services
    /// and applies them to the request localization middleware within the pipeline.
    /// </remarks>
    public static void UseCustomLocalization(this IApplicationBuilder app)
    {
        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);
    }
}