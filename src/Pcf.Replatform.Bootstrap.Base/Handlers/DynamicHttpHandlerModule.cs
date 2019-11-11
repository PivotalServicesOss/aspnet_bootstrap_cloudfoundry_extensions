using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public class DynamicHttpHandlerModule : IHttpModule
    {
        internal static List<DynamicHttpHandlerBase> Handlers { get; } = new List<DynamicHttpHandlerBase>();

        private ILogger<DynamicHttpHandlerModule> logger;

        public DynamicHttpHandlerModule() { }

        public void Init(HttpApplication application)
        {
            logger = DependencyContainer.GetService<ILogger<DynamicHttpHandlerModule>>(true);

            var asyncHandlerHelper = new EventHandlerTaskAsyncHelper(FilterAndPreProcessRequest);

            foreach (var handler in Handlers)
                handler.RegisterEvent(application, asyncHandlerHelper);
        }

        private async Task FilterAndPreProcessRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            await FilterAndPreProcessRequest(context, HttpContext.Current.ApplicationInstance.CompleteRequest)
                            .ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task FilterAndPreProcessRequest(HttpContextBase context, Action completeRequest)
        {
            foreach (var handler in Handlers)
            {
                if (handler.IsPathMatched(context))
                {
                    if (await handler.IsEnabledAsync(context).ConfigureAwait(continueOnCapturedContext: false))
                        handler.HandleRequest(context);

                    if (!await handler.ContinueNextAsync(context))
                        completeRequest();

                    break;
                }
            }
        }

        public void Dispose()
        { }
    }
}