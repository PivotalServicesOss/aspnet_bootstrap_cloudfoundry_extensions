using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using System;
using System.Web;

//Minimum tests are added
namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Diagnostics
{
    [TestClass]
    public class InboundRequestObserverModuleTests
    {
        Mock<ILoggerFactory> loggerFactory;
        Mock<IHost> host;

        public InboundRequestObserverModuleTests()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(InboundRequestObserverModule))).Returns(new Mock<ILogger<InboundRequestObserverModule>>().Object);
        }

        [TestMethod]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.IsTrue(new InboundRequestObserverModule() is IHttpModule);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull_Context_EndRequest()
        {
            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
            TestHelper.SetNonPublicStaticFieldValue(typeof(AppConfig), "host", host.Object);
            new InboundRequestObserverModule().InvokeNonPublicInstanceMethod("Context_EndRequest", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull_Context_BeginRequest()
        {
            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
            TestHelper.SetNonPublicStaticFieldValue(typeof(AppConfig), "host", host.Object);
            new InboundRequestObserverModule().InvokeNonPublicInstanceMethod("Context_BeginRequest", null, null);
        }
    }
}
