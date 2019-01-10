using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Extensions
{
    [TestClass]
    public class ConfigurationBuilderExtensionsTests
    {
        [TestMethod]
        public void Test_If_AddWebConfiguration_Adds_WebConfigurationSource()
        {
            var builder = new ConfigBuilderStub();
            builder.AddWebConfiguration();

            Assert.IsTrue(builder.Sources.Any((s) => { return s is WebConfigurationSource; }));
        }

        [TestMethod]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource()
        {
            var builder = new ConfigBuilderStub();
            builder.AddInMemoryConfiguration();

            Assert.IsTrue(builder.Sources.Any((s) => { return s is InMemoryConfigurationSource; }));
        }

        [TestMethod]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource_WithGivenSourceReference()
        {
            var builder = new ConfigBuilderStub();
            builder.AddInMemoryConfiguration(new Dictionary<string, string>());

            Assert.IsTrue(builder.Sources.Any((s) => { return s is InMemoryConfigurationSource; }));
        }
    }
}
