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
    public class ScopedLoggingModuleTests
    {
        Mock<ILoggerFactory> loggerFactory;
        Mock<IHost> host;

        public ScopedLoggingModuleTests()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(ScopedLoggingModule))).Returns(new Mock<ILogger<ScopedLoggingModule>>().Object);
        }

        [TestMethod]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.IsTrue(new ScopedLoggingModule() is IHttpModule);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull_Context_BeginRequest()
        {
            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
            TestHelper.SetNonPublicStaticFieldValue(typeof(AppConfig), "host", host.Object);
            new ScopedLoggingModule().InvokeNonPublicInstanceMethod("Context_BeginRequest", null, null);
        }
    }
}
