using Kerberos.NET;
using Kerberos.NET.Crypto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public class SpnegoAuthenticator: IAuthenticator
    {
        private const string SchemeName = "Negotiate";
        private readonly IConfiguration configuration;
        private readonly ILogger<SpnegoAuthenticator> logger;
        private KerberosAuthenticator authenticator;

        public SpnegoAuthenticator(IConfiguration configuration, ILogger<SpnegoAuthenticator> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            CreateAuthenticator();
        }

        private void CreateAuthenticator()
        {
            if (string.IsNullOrWhiteSpace(configuration["PRINCIPAL_PASSWORD"]))
                throw new ArgumentNullException($"PRINCIPAL_PASSWORD");

            authenticator = new KerberosAuthenticator(new KerberosValidator(new KerberosKey(configuration["PRINCIPAL_PASSWORD"])))
            {
                UserNameFormat = UserNameFormat.DownLevelLogonName
            };
        }

        public async Task<AuthenticateResult> AuthenticateAsync(HttpContextBase contextBase)
        {
            AuthenticateResult authenticateResult = await HandleAuthenticateAsync(contextBase);

            if (authenticateResult?.Failure == null)
            {
                if ((authenticateResult?.Ticket)?.Principal != null)
                    logger.LogDebug($"AuthenticationSchemeAuthenticated, {SchemeName}");
                else
                    logger.LogError($"AuthenticationSchemeNotAuthenticated, {SchemeName}");
            }
            else
                logger.LogError($"AuthenticationSchemeNotAuthenticatedWithFailure, {SchemeName}, {authenticateResult.Failure.Message}");

            return authenticateResult;
        }

        private async Task<AuthenticateResult> HandleAuthenticateAsync(HttpContextBase contextBase)
        {
            if (contextBase == null)
                throw new ArgumentNullException(nameof(contextBase));

            string authorizationHeader = contextBase.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.NoResult();

            if (!authorizationHeader.StartsWith("Negotiate ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var base64Token = authorizationHeader.Substring(SchemeName.Length).Trim();

            if (string.IsNullOrEmpty(base64Token))
            {
                const string noCredentialsMessage = "No credentials";
                logger.LogInformation(noCredentialsMessage);
                return AuthenticateResult.Fail(noCredentialsMessage);
            }

            try
            {
                try
                {
                    logger.LogTrace("===SPNEGO Token===");
                    logger.LogTrace(base64Token);
                    var identity = await authenticator.Authenticate(base64Token);
                    var ticket = new AuthenticationTicket(
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties(),
                        "Negotiate");
                    return AuthenticateResult.Success(ticket);
                }
                catch (KerberosValidationException e)
                {
                    return AuthenticateResult.Fail(e);
                }
            }
            catch (Exception exception)
            {
                return AuthenticateResult.Fail("Access denied");
            }
        }

        public async Task ChallengeAsync(AuthenticationProperties properties, HttpContextBase contextBase)
        {
            properties = (properties ?? new AuthenticationProperties());
            await HandleChallengeAsync(properties, contextBase);
            logger.LogError($"AuthenticationSchemeChallenged, {SchemeName}");
        }

        private Task HandleChallengeAsync(AuthenticationProperties properties, HttpContextBase contextBase)
        {
            contextBase.Response.StatusCode = 401;
            contextBase.Response.Headers.Set(HeaderNames.WWWAuthenticate, "Negotiate");
            return Task.CompletedTask;
        }
    }
}