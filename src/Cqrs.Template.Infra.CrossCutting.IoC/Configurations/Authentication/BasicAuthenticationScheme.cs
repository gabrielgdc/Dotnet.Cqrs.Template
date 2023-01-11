using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;

public class BasicAuthenticationScheme : AuthenticationHandler<BasicAuthenticationSchemeOptions>
{
    public BasicAuthenticationScheme(
        IOptionsMonitor<BasicAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() => throw new System.NotImplementedException();
}

public class BasicAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}
