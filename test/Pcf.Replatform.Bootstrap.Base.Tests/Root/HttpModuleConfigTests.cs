using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using System.Linq;

//Minimum tests are added
namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Root
{
    [TestClass]
    public class HttpModuleConfigTests
    {
        [TestMethod]
        public void Test_If_GetModuleTypes_Returns_4_Modules()
        {
            Assert.AreEqual(4, HttpModuleConfig.GetModuleTypes().Count());
        }

        [TestMethod]
        public void Test_If_GetModuleTypes_Returns_InboundRequestObserverModule()
        {
            Assert.IsTrue(HttpModuleConfig.GetModuleTypes().Any((m) => { return m == typeof(InboundRequestObserverModule); }));
        }

        [TestMethod]
        public void Test_If_GetModuleTypes_Returns_ScopedLoggingModule()
        {
            Assert.IsTrue(HttpModuleConfig.GetModuleTypes().Any((m) => { return m == typeof(ScopedLoggingModule); }));
        }

        [TestMethod]
        public void Test_If_GetModuleTypes_Returns_GlobalErrorHandlerModule()
        {
            Assert.IsTrue(HttpModuleConfig.GetModuleTypes().Any((m) => { return m == typeof(GlobalErrorHandlerModule); }));
        }

        [TestMethod]
        public void Test_If_GetModuleTypes_Returns_RequestLoggerModule()
        {
            Assert.IsTrue(HttpModuleConfig.GetModuleTypes().Any((m) => { return m == typeof(RequestLoggerModule); }));
        }
    }
}
