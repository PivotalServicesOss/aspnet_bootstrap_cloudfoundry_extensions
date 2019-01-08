using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using Steeltoe.Management.Tracing.Observer;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Diagnostics
{
    [TestClass]
    public class OutboundRequestObserverTests
    {
        Mock<ITracing> tracing;
        Mock<ITracingOptions> tracingOptions;
        Mock<ILoggerFactory> loggerFactory;

        public OutboundRequestObserverTests()
        {
            tracing = new Mock<ITracing>();
            tracingOptions = new Mock<ITracingOptions>();
            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(OutboundRequestObserver))).Returns(new Mock<ILogger<OutboundRequestObserver>>().Object);
        }

        [TestMethod]
        public void Test_IsTypeOfHttpClientDesktopObserver()
        {
            Assert.IsTrue(new OutboundRequestObserver(tracingOptions.Object, tracing.Object, loggerFactory.Object) is HttpClientDesktopObserver);
        }
    }
}
