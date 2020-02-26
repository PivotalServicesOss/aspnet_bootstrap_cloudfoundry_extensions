using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers
{
    public class InboundErrorRequestObserverHandler : DynamicHttpHandlerBase
    {
        IInboundRequestObserver observer;

        public InboundErrorRequestObserverHandler(IInboundRequestObserver observer, ILogger<InboundErrorRequestObserverHandler> logger)
             : base(logger)
        {
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.Error;

        public override void HandleRequest(HttpContextBase context)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequestBase>(context, "Request");

            observer.ProcessEvent(InboundRequestObserver.ERR_EVNT, context);
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }
    }
}
