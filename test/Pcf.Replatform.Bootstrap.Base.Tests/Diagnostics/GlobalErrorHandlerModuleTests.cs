using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Diagnostics
{
    [TestClass]
    public class GlobalErrorHandlerModuleTests
    {
        Mock<ILoggerFactory> loggerFactory;
        Mock<IHost> host;

        public GlobalErrorHandlerModuleTests()
        {
            loggerFactory = new Mock<ILoggerFactory>();
            loggerFactory.Setup(l => l.CreateLogger(nameof(GlobalErrorHandlerModule))).Returns(new Mock<ILogger<GlobalErrorHandlerModule>>().Object);

            host = new Mock<IHost>();
            var services = new ServiceCollection();
            host.SetupGet(h => h.Services).Returns(services.BuildServiceProvider());
        }

        [TestMethod]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.IsTrue(new GlobalErrorHandlerModule() is IHttpModule);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_ThrowsExceptionIfLoggerFactoryIsNull()
        {
            Assert.IsTrue(new GlobalErrorHandlerModule() is IHttpModule);
        }
    }
}
