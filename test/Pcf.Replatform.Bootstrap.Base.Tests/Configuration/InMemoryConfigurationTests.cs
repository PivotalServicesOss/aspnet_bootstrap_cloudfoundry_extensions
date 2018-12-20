using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using System.Collections.Generic;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Configuration
{
    [TestClass]
    public class InMemoryConfigurationTests
    {
        [TestMethod]
        public void Test_LoadConfigurationFromInMemoryConfiguration()
        {
            var store = new Dictionary<string, string>
            {
                {"key1", "value1" },
                {"key1:subkey1", "value11" },
            };

            var provider = new InMemoryConfigurationProvider(store);
            provider.Load();

            Assert.IsTrue(provider.TryGet("key1", out string value));
            Assert.AreEqual("value1", value);

            Assert.IsTrue(provider.TryGet("key1:subkey1", out string value1));
            Assert.AreEqual("value11", value1);
        }
    }
}
