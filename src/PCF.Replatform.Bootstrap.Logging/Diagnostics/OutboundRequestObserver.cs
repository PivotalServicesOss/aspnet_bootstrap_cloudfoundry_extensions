using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using Steeltoe.Management.Tracing.Observer;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
{
    public class OutboundRequestObserver : HttpClientDesktopObserver
    {
        public OutboundRequestObserver(ITracingOptions tracingOptions, ITracing tracing, ILoggerFactory loggerFactory)
            : base(tracingOptions, tracing, loggerFactory.CreateLogger<OutboundRequestObserver>())
        {
        }

        public override void ProcessEvent(string evnt, object arg)
        {
            if (!AppBuilderExtensions.IncludeCorrelation)
                return;

            base.ProcessEvent(evnt, arg);
        }
    }
}
