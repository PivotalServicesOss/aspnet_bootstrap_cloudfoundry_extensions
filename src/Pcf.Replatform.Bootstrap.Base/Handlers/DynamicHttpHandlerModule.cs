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
        internal static List<IDynamicHttpHandler> Handlers { get; } = new List<IDynamicHttpHandler>();
        private ILogger<DynamicHttpHandlerModule> logger;

        public DynamicHttpHandlerModule() { }

        public void Init(HttpApplication context)
        {
            logger = DependencyContainer.GetService<ILogger<DynamicHttpHandlerModule>>(true);

            EventHandlerTaskAsyncHelper eventHandlerTaskAsyncHelper = new EventHandlerTaskAsyncHelper(FilterAndPreProcessRequest);
            context.AddOnPostAuthorizeRequestAsync(eventHandlerTaskAsyncHelper.BeginEventHandler, eventHandlerTaskAsyncHelper.EndEventHandler);
        }

        private async Task FilterAndPreProcessRequest(HttpContextBase context, Action completeRequest)
        {
            foreach (var handler in Handlers)
            {
                if (handler.IsPathMatched(context))
                {
                    if (await handler.IsAllowedAsync(context).ConfigureAwait(continueOnCapturedContext: false))
                        handler.HandleRequest(context);

                    completeRequest();
                    break;
                }
            }
        }

        private async Task FilterAndPreProcessRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            await FilterAndPreProcessRequest(context, HttpContext.Current.ApplicationInstance.CompleteRequest)
                            .ConfigureAwait(continueOnCapturedContext: false);
        }

        public void Dispose()
        { }
    }
}