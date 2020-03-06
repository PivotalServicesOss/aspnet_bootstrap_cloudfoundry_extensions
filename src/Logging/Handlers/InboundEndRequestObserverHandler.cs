using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System;
using System.Web;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers
{
    public class InboundEndRequestObserverHandler : DynamicHttpHandlerBase
    {
        IInboundRequestObserver observer;

        public InboundEndRequestObserverHandler(IInboundRequestObserver observer, ILogger<InboundEndRequestObserverHandler> logger)
             : base(logger)
        {
            this.observer = observer ?? throw new ArgumentNullException(nameof(observer));
        }

        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.EndRequest;

        public override void HandleRequest(HttpContextBase context)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequestBase>(context, "Request");

            observer.ProcessEvent(InboundRequestObserver.STOP_EVNT, context);
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }
    }
}
