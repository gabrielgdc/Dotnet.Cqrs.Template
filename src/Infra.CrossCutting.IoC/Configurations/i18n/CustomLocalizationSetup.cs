using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infra.CrossCutting.IoC.Configurations.i18n;

public static class CustomLocalizationSetup
{
    public static void AddCustomLocalization(this IServiceCollection services)
    {
        services.AddLocalization();

        services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new[]
            {
                new CultureInfo("pt-br"), 
                new CultureInfo("en-us")
            };

            options.DefaultRequestCulture = new RequestCulture(culture: "pt-br");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
    }

    public static void UseCustomLocalization(this IApplicationBuilder app)
    {
        var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(localizationOptions.Value);
    }
}
