using Kerberos.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public class SpnegoAuthenticator : ISpnegoAuthenticator
    {
        private readonly ITicketIssuer issuer;
        private readonly ILogger<SpnegoAuthenticator> logger;

        public SpnegoAuthenticator(ITicketIssuer issuer, ILogger<SpnegoAuthenticator> logger)
        {
            this.issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public AuthenticateResult Authenticate(HttpContextBase contextBase)
        {
            if (contextBase == null)
                throw new ArgumentNullException(nameof(contextBase));

            string authorizationHeader = contextBase.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.NoResult();

            if (!authorizationHeader.StartsWith($"{AuthConstants.SPNEGO_DEFAULT_SCHEME} ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var base64Token = authorizationHeader.Substring(AuthConstants.SPNEGO_DEFAULT_SCHEME.Length).Trim();
            if (string.IsNullOrEmpty(base64Token))
            {
                const string noCredentialsMessage = "No credentials";
                logger.LogWarning(noCredentialsMessage);
                return AuthenticateResult.Fail(noCredentialsMessage);
            }

            try
            {
                try
                {
                    logger.LogTrace($"SPNEGO Token: {base64Token}");
                    var ticket = issuer.Authenticate(base64Token);
                    logger.LogDebug($"Authenticated successfully, kerberos ticket recieved...");
                    return AuthenticateResult.Success(ticket);
                }
                catch (KerberosValidationException e)
                {
                    return AuthenticateResult.Fail(e);
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Access denied!");
            }
        }

        public void Challenge(AuthenticationProperties properties, HttpContextBase contextBase)
        {
            properties = properties ?? new AuthenticationProperties();
            contextBase.Response.StatusCode = 401;
            contextBase.Response.Headers.Set(HeaderNames.WWWAuthenticate, AuthConstants.SPNEGO_DEFAULT_SCHEME);
            logger.LogDebug("Issuing WWW-Authenticate challenge");
        }
    }
}