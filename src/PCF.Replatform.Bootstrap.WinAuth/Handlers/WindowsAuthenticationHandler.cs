using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Threading.Tasks;
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

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.AuthenticateRequest;

        public override void HandleRequest(HttpContextBase contextBase)
        {
            var cookieAuthResult = cookieAuthenticator.Authenticate(contextBase);
            if (cookieAuthResult.Succeeded)
            {
                contextBase.User = cookieAuthResult.Ticket.Principal;
                return;
            }

            var spnegoAuthResult = spnegoAuthenticator.Authenticate(contextBase);

            if(spnegoAuthResult.Succeeded)
            {
                cookieAuthenticator.SignIn(spnegoAuthResult, contextBase);
                contextBase.User = spnegoAuthResult.Ticket.Principal;
                return;
            }
            else
            {
                logger.LogDebug("User authentication failed, issuing WWW-Authenticate challenge");
                spnegoAuthenticator.Challenge(new AuthenticationProperties(), contextBase);
                return;
            }
        }

        private bool IsAuthenticated(HttpContextBase context)
        {
            return !(context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated);
        }

        public override async Task<bool> ContinueNextAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: IsAuthenticated(context));
        }
    }
}
