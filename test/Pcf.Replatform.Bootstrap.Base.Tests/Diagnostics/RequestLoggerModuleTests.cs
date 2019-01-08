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
    public class RequestLoggerModuleTests
    {
        Mock<ILoggerFactory> loggerFactory;
        Mock<IHost> host;

        public RequestLoggerModuleTests()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(RequestLoggerModule))).Returns(new Mock<ILogger<RequestLoggerModule>>().Object);
        }

        [TestMethod]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.IsTrue(new RequestLoggerModule() is IHttpModule);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull_Context_EndRequest()
        {
            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
            TestHelper.SetNonPublicStaticFieldValue(typeof(AppConfig), "host", host.Object);
            new RequestLoggerModule().InvokeNonPublicInstanceMethod("Context_EndRequest", null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull_Context_BeginRequest()
        {
            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
            TestHelper.SetNonPublicStaticFieldValue(typeof(AppConfig), "host", host.Object);
            new RequestLoggerModule().InvokeNonPublicInstanceMethod("Context_BeginRequest", null, null);
        }
    }
}
