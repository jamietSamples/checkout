using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Payment.Api.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        private const string ApiKeyName = "ApiKey";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            ) : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyName, out var apiKeyValue))
            {
                return Task.FromResult(AuthenticateResult.Fail("ApiKey Not Present"));
            }

            if (string.IsNullOrEmpty(apiKeyValue))
            {
                return Task.FromResult(AuthenticateResult.Fail("ApiKey contained no value"));
            }

            var appSettings = Context.Request.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(ApiKeyName);

            if (!apiKey.Equals(apiKeyValue))
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid ApiKey value"));
            }

            var claims = new[] { new Claim(ClaimTypes.Name, "ApiKey") };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            return Task.FromResult(Response.StatusCode = 401);
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            return Task.FromResult(Response.StatusCode = 403);
        }
    }
}
