using Microsoft.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing.Observer;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Observers
{
    public class OutboundRequestObserver : HttpClientDesktopObserver
    {
        public OutboundRequestObserver(ITracingOptions tracingOptions, ITracing tracing, ILoggerFactory loggerFactory)
            : base(tracingOptions, tracing, loggerFactory.CreateLogger<OutboundRequestObserver>())
        {
        }

        public override void ProcessEvent(string evnt, object arg)
        {
            if (!LoggingConstants.IncludeDistributedTracing)
                return;

            base.ProcessEvent(evnt, arg);
        }
    }
}
