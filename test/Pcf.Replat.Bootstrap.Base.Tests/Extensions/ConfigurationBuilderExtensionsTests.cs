using PCF.Replatform.Test.Helpers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Pcf.Replat.Bootstrap.Base.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource()
        {
            var builder = new ConfigBuilderStub();

            TestProxy.AddInMemoryConfiguration(builder);

            Assert.Contains(builder.Sources, (s) => { return s is InMemoryConfigurationSource; });
        }

        [Fact]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource_WithGivenSourceReference()
        {
            var builder = new ConfigBuilderStub();

            TestProxy.AddInMemoryConfiguration(builder, new Dictionary<string, string>());

            Assert.Contains(builder.Sources, (s) => { return s is InMemoryConfigurationSource; });
        }
    }
}
