using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Cqrs.Template.Infra.CrossCutting.Environments.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;

namespace Cqrs.Template.Infra.CrossCutting.IoC.Configurations.Authentication;

public class BasicAuthenticationScheme : AuthenticationHandler<BasicAuthenticationSchemeOptions>
{
    private readonly BasicAuthenticationConfiguration _basicAuthenticationConfiguration;

    public BasicAuthenticationScheme(
        IOptionsMonitor<BasicAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<BasicAuthenticationConfiguration> basicAuthenticationConfiguration
    ) : base(options, logger, encoder, clock)
    {
        _basicAuthenticationConfiguration = basicAuthenticationConfiguration.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("Crendenciais inválidas"));
            }

            var authorization = authorizationHeader.ToString();

            if (!authorization.Contains("Basic", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.Fail("Crendenciais inválidas"));
            }

            var basicAuth = authorization.Replace("Basic", "", StringComparison.OrdinalIgnoreCase).Trim();

            var authString = Encoding.ASCII.GetString(Convert.FromBase64String(basicAuth));

            var credentials = authString.Split(':');

            var (username, password) = (credentials[0], credentials[1]);

            if (!username.Equals(_basicAuthenticationConfiguration.Username) || !password.Equals(_basicAuthenticationConfiguration.Password))
            {
                return Task.FromResult(AuthenticateResult.Fail("Crendenciais inválidas"));
            }

            var ticket = GetAuthenticationTicket(new List<Claim>());
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (Exception e)
        {
            Logger.LogCritical("Ocorreu um erro ao autenticar a requisição #### Exception: {0}, StackTrace: {1} ####", e.Message, e.StackTrace);
            return Task.FromResult(AuthenticateResult.Fail("Crendenciais inválidas"));
        }
    }

    private AuthenticationTicket GetAuthenticationTicket(IEnumerable<Claim> claims)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
        claimsPrincipal.AddIdentity(claimsIdentity);

        return new AuthenticationTicket(claimsPrincipal, Scheme.Name);
    }
}

public class BasicAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}
