using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public class WindowsAuthenticationHandler : DynamicHttpHandlerBase
    {
        public WindowsAuthenticationHandler(ILogger<WindowsAuthenticationHandler> logger)
            : base(logger)
        {
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.AuthenticateRequest;

        public override void HandleRequest(HttpContextBase context)
        {
            if (!IsAuthenticatedAlready(context))
            {
                var authenticators = DependencyContainer.GetService<IEnumerable<IAuthenticator>>().ToList();

                var spnegoAuthenticator = authenticators.Single(h => h is SpnegoAuthenticator);
                //var cookieAuthHandler = (CookieAuthenticationHandler)handlers.Single(h => h is CookieAuthenticationHandler);

                var authResult = spnegoAuthenticator.AuthenticateAsync(context).GetAwaiter().GetResult();

                if (authResult.Succeeded)
                {
                    logger.LogDebug($"User {authResult.Principal.Identity.Name} successfully logged in");
                    //cookieAuthHandler.SignInAsync(authResult.Principal, new AuthenticationProperties()).GetAwaiter().GetResult();
                    context.User = authResult.Principal;
                }
                else
                {
                    logger.LogDebug("User authentication failed, issuing WWW-Authenticate challenge");
                    spnegoAuthenticator.ChallengeAsync(new AuthenticationProperties(), context).GetAwaiter().GetResult();
                    return;
                }
            }
        }

        private bool IsAuthenticatedAlready(HttpContextBase context)
        {
            return !(context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated);
        }

        public override async Task<bool> ContinueNextAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: IsAuthenticatedAlready(context));
        }
    }
}
