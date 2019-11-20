using System;
using System.IO;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Kerberos.NET;
using Kerberos.NET.Crypto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Options;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public class SpnegoAuthenticationHandler : AuthenticationHandler<SpnegoAuthenticationOptions>
    {
        private const string SchemeName = "Negotiate";
        private KerberosAuthenticator _authenticator;
        private readonly IDisposable _monitorHandle;


        public SpnegoAuthenticationHandler(
            IOptionsMonitor<SpnegoAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, loggerFactory, encoder, clock)
        {
            _monitorHandle = options.OnChange(CreateAuthenticator);
        }

        private void CreateAuthenticator(SpnegoAuthenticationOptions options)
        {
            if (Options.PrincipalPassword != null)
                _authenticator = new KerberosAuthenticator(new KerberosValidator(new KerberosKey(options.PrincipalPassword)));
            else
                _authenticator = new KerberosAuthenticator(new KeyTable(File.ReadAllBytes(Options.KeytabFile)));
            _authenticator.UserNameFormat = UserNameFormat.DownLevelLogonName;
        }

        public HttpContextBase ContextBase { get; set; }

        public new async Task<AuthenticateResult> AuthenticateAsync()
        {
            AuthenticateResult authenticateResult = await HandleAuthenticateOnceAsync();

            if (authenticateResult?.Failure == null)
            {
                if ((authenticateResult?.Ticket)?.Principal != null)
                {
                    Logger.LogDebug($"AuthenticationSchemeAuthenticated, {SchemeName}");
                }
                else
                {
                    Logger.LogError($"AuthenticationSchemeNotAuthenticated, {SchemeName}");
                }
            }
            else
            {
                Logger.LogError($"AuthenticationSchemeNotAuthenticatedWithFailure, {SchemeName}, {authenticateResult.Failure.Message}");
            }
            return authenticateResult;
        }

        public new async Task ChallengeAsync(AuthenticationProperties properties)
        {
            properties = (properties ?? new AuthenticationProperties());
            await HandleChallengeAsync(properties);
            Logger.LogError($"AuthenticationSchemeChallenged, {SchemeName}");
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (ContextBase == null)
                throw new ArgumentNullException(nameof(ContextBase));

            string authorizationHeader = ContextBase.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader))
                return AuthenticateResult.NoResult();

            if (!authorizationHeader.StartsWith("Negotiate ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.NoResult();

            var base64Token = authorizationHeader.Substring(SchemeName.Length).Trim();

            if (string.IsNullOrEmpty(base64Token))
            {
                const string noCredentialsMessage = "No credentials";
                Logger.LogInformation(noCredentialsMessage);
                return AuthenticateResult.Fail(noCredentialsMessage);
            }

            try
            {
                try
                {
                    Logger.LogTrace("===SPNEGO Token===");
                    Logger.LogTrace(base64Token);
                    var identity = await _authenticator.Authenticate(base64Token);
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
            catch (Exception)
            {
                return AuthenticateResult.Fail("Access denied");
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            ContextBase.Response.StatusCode = 401;
            ContextBase.Response.Headers.Set(HeaderNames.WWWAuthenticate, "Negotiate");
            return Task.CompletedTask;
        }

        protected override Task InitializeHandlerAsync()
        {
            CreateAuthenticator(Options);
            return Task.CompletedTask;
        }
    }
}