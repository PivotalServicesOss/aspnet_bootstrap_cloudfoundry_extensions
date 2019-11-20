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
            if (context.User == null || context.User.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                var handlers = DependencyContainer.GetService<IEnumerable<IAuthenticationHandler>>().ToList();

                var spnegoAuthHandler = (SpnegoAuthenticationHandler)handlers.Single(h => h is SpnegoAuthenticationHandler);
                var cookieAuthHandler = (CookieAuthenticationHandler)handlers.Single(h => h is CookieAuthenticationHandler);

                spnegoAuthHandler.ContextBase = context;

                var authResult = spnegoAuthHandler.AuthenticateAsync().GetAwaiter().GetResult();

                if (authResult.Succeeded)
                {
                    logger.LogDebug($"User {authResult.Principal.Identity.Name} successfully logged in");
                    cookieAuthHandler.SignInAsync(authResult.Principal, new AuthenticationProperties()).GetAwaiter().GetResult();
                    context.User = authResult.Principal;
                }
                else
                {
                    logger.LogDebug("User authentication failed, issuing WWW-Authenticate challenge");
                    spnegoAuthHandler.ChallengeAsync(new AuthenticationProperties()).GetAwaiter().GetResult();
                    return;
                }
            }
        }

        public override async Task<bool> ContinueNextAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: true);
        }
    }
}
