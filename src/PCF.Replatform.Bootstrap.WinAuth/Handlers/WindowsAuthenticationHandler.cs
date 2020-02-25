using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public class WindowsAuthenticationHandler : DynamicHttpHandlerBase
    {
        private readonly ICookieAuthenticator cookieAuthenticator;
        private readonly ISpnegoAuthenticator spnegoAuthenticator;

        public WindowsAuthenticationHandler(ICookieAuthenticator cookieAuthenticator, ISpnegoAuthenticator spnegoAuthenticator, ILogger<WindowsAuthenticationHandler> logger)
            : base(logger)
        {
            this.cookieAuthenticator = cookieAuthenticator ?? throw new ArgumentNullException(nameof(cookieAuthenticator));
            this.spnegoAuthenticator = spnegoAuthenticator ?? throw new ArgumentNullException(nameof(spnegoAuthenticator));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.PostAuthenticateRequest;

        public override void HandleRequest(HttpContextBase contextBase)
        {
            if (IsAuthenticated(contextBase))
                return;

            var cookieAuthResult = cookieAuthenticator.Authenticate(contextBase);
            if (cookieAuthResult.Succeeded)
            {
                contextBase.User = cookieAuthResult.Ticket.Principal;
                logger.LogDebug($"Logged in user (cookie): {contextBase.User.Identity.Name}");
                return;
            }

            var spnegoAuthResult = spnegoAuthenticator.Authenticate(contextBase);

            if (spnegoAuthResult.Succeeded)
            {
                cookieAuthenticator.SignIn(spnegoAuthResult, contextBase);
                contextBase.User = spnegoAuthResult.Ticket.Principal;
                logger.LogDebug($"Logged in user (spnego): {contextBase.User.Identity.Name}");
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
            return IsAuthenticated(context);
        }
    }
}
