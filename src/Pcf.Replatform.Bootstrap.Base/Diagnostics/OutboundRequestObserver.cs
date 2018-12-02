using Microsoft.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using Steeltoe.Management.Tracing.Observer;

namespace Pcf.Replatform.Bootstrap.Diagnostics
{
    public class OutboundRequestObserver : HttpClientDesktopObserver
    {
        public OutboundRequestObserver(ITracingOptions tracingOptions, ITracing tracing, ILoggerFactory loggerFactory)
            : base(tracingOptions, tracing, loggerFactory.CreateLogger<OutboundRequestObserver>())
        {
        }
    }
}
