using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using trsaints_frontend_api.Constants;

namespace trsaints_frontend_api.Authorization.Middleware;

public class
    ApiKeyAuthenticationHandler : AuthenticationHandler<
    AuthenticationSchemeOptions>
{
    private readonly string _apiKeyHeaderName;
    private readonly string _apiKey;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options,
               logger,
               encoder,
               clock)
    {
        _apiKeyHeaderName = ApiKeyAccessConstants.ApiKeyHeaderName;
        _apiKey =
            configuration.GetValue<string>(
                ApiKeyAccessConstants.ApiKeyName);
    }

    protected override async Task<AuthenticateResult>
        HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(_apiKeyHeaderName,
                                         out var apiKeyHeaderValues))
            return AuthenticateResult.NoResult();

        if (_apiKey == null)
            return AuthenticateResult.Fail("API Key was not provided.");

        var providedApiKey =
            System.Net.WebUtility.UrlDecode(
                apiKeyHeaderValues.FirstOrDefault());

        if (providedApiKey != _apiKey)
            return AuthenticateResult.Fail("API Key is invalid.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, providedApiKey)
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}
