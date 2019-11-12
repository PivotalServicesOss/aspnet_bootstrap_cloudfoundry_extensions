using Microsoft.Extensions.Logging;
using Moq;
using OpenCensus.Trace;
using OpenCensus.Trace.Propagation;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing.Observer;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class OutboundRequestObserverTests
    {
        Mock<ITracing> tracing;
        Mock<ITracingOptions> tracingOptions;
        Mock<ILoggerFactory> loggerFactory;
        Mock<ITextFormat> textFormat;
        Mock<IPropagationComponent> propogationComponent;
        Mock<ITracer> tracer;

        public OutboundRequestObserverTests()
        {
            tracing = new Mock<ITracing>();
            tracingOptions = new Mock<ITracingOptions>();
            textFormat = new Mock<ITextFormat>();
            propogationComponent = new Mock<IPropagationComponent>();
            tracer = new Mock<ITracer>();

            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(OutboundRequestObserver))).Returns(new Mock<ILogger<OutboundRequestObserver>>().Object);

            tracingOptions.SetupGet(to => to.EgressIgnorePattern).Returns(string.Empty);

            tracing.SetupGet(t => t.PropagationComponent).Returns(propogationComponent.Object);
            tracing.SetupGet(t => t.Tracer).Returns(tracer.Object);

            propogationComponent.SetupGet(pc => pc.TextFormat).Returns(textFormat.Object);
        }

        [Fact]
        public void Test_IsTypeOfHttpClientDesktopObserver()
        {
            Assert.True(new OutboundRequestObserver(tracingOptions.Object, tracing.Object, loggerFactory.Object) is HttpClientDesktopObserver);
        }
    }
}
