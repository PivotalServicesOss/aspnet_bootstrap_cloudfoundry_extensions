using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Linq;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public class WindowsAuthenticationHandler : DynamicHttpHandlerBase
    {
        private readonly ICookieAuthenticator cookieAuthenticator;
        private readonly ISpnegoAuthenticator spnegoAuthenticator;
        private readonly IConfiguration configuration;
        public const string WHITELIST_PATHS_CSV_DEFAULT = "/cloudfoundryapplication,/cloudfoundryapplication/,/actuator,/actuator/";

        public WindowsAuthenticationHandler(ICookieAuthenticator cookieAuthenticator, ISpnegoAuthenticator spnegoAuthenticator, IConfiguration configuration, ILogger<WindowsAuthenticationHandler> logger)
            : base(logger)
        {
            this.cookieAuthenticator = cookieAuthenticator ?? throw new ArgumentNullException(nameof(cookieAuthenticator));
            this.spnegoAuthenticator = spnegoAuthenticator ?? throw new ArgumentNullException(nameof(spnegoAuthenticator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.PostAuthenticateRequest;

        public override void HandleRequest(HttpContextBase contextBase)
        {
            if (IsWhitelisted(contextBase))
                return;

            if (IsAuthenticated(contextBase))
                return;

            var cookieAuthResult = cookieAuthenticator.Authenticate(contextBase);
            if (cookieAuthResult.Succeeded)
            {
                contextBase.User = cookieAuthResult.Ticket.Principal;
                logger.LogDebug($"Logged in user (cookie): {cookieAuthResult.Ticket.Principal}");
                return;
            }

            var spnegoAuthResult = spnegoAuthenticator.Authenticate(contextBase);

            if (spnegoAuthResult.Succeeded)
            {
                cookieAuthenticator.SignIn(spnegoAuthResult, contextBase);
                contextBase.User = spnegoAuthResult.Ticket.Principal;
                logger.LogDebug($"Logged in user (spnego): {spnegoAuthResult.Ticket.Principal}");
                return;
            }
            else
            {
                spnegoAuthenticator.Challenge(new AuthenticationProperties(), contextBase);
                return;
            }
        }

        private bool IsAuthenticated(HttpContextBase context)
        {
            var isAuthenticated = !(context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated);
            logger.LogDebug($"Is Authenticated: {isAuthenticated}");
            return isAuthenticated;
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return IsWhitelisted(context) || IsAuthenticated(context);
        }

        private bool IsWhitelisted(HttpContextBase context)
        {
            var whitelistedPaths = WHITELIST_PATHS_CSV_DEFAULT.Split(',').ToList();

            if (!string.IsNullOrWhiteSpace(configuration[AuthConstants.WHITELIST_PATHS_CSV_NM]))
                whitelistedPaths.AddRange(configuration[AuthConstants.WHITELIST_PATHS_CSV_NM].Split(',').ToList());

            foreach (var path in whitelistedPaths)
            {
                if (context.Request.Path.Contains(path))
                    return true;
            }
            return false;
        }
    }
}
