using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Server.Authentication;

public class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "ApiKey";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var expected = configuration["MeetingApi:ApiKey"];
        if (string.IsNullOrWhiteSpace(expected))
            return Task.FromResult(AuthenticateResult.Fail("API key is not configured."));

        if (!Request.Headers.TryGetValue("X-API-Key", out var keys))
            return Task.FromResult(AuthenticateResult.NoResult());

        if (keys.Count != 1 || string.IsNullOrWhiteSpace(keys[0])
            || !KeysMatch(keys[0]!, expected))
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "meeting-api-client"),
            new Claim(ClaimTypes.Name, "Meeting API Client")
        }, SchemeName);

        var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.Headers["WWW-Authenticate"] = SchemeName;
        return Response.WriteAsJsonAsync(new
        {
            success = false,
            message = "مشکل در تنظیمات اولیه"
        });
    }

    private static bool KeysMatch(string received, string expected)
        => CryptographicOperations.FixedTimeEquals(
            SHA256.HashData(Encoding.UTF8.GetBytes(received)),
            SHA256.HashData(Encoding.UTF8.GetBytes(expected)));
}
