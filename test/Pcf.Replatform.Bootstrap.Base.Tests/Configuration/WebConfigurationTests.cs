using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using System.Collections.Generic;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Configuration
{
    [TestClass]
    public class WebConfigurationTests
    {
        [TestMethod]
        public void Test_AppSettingsFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.IsTrue(provider.TryGet("AppSettings:appkey1", out string value));
            Assert.AreEqual("appvalue1", value);
        }

        [TestMethod]
        public void Test_ConnectionStringFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.IsTrue(provider.TryGet("ConnectionStrings:conn1", out string value));
            Assert.AreEqual("my dummy connection string", value);
        }

        [TestMethod]
        public void Test_ProvidersFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.IsTrue(provider.TryGet("Providers:conn1", out string value));
            Assert.AreEqual("provider1", value);
        }
    }
}
