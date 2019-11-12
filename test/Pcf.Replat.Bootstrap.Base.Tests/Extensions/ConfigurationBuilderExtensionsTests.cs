using PCF.Replatform.Test.Helpers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace Pcf.Replat.Bootstrap.Base.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource()
        {
            var builder = new ConfigBuilderStub();

            TestProxy.AddInMemoryConfigurationProxy(builder);

            Assert.Contains(builder.Sources, (s) => { return s is InMemoryConfigurationSource; });
        }

        [Fact]
        public void Test_If_AddInMemoryConfiguration_Adds_InMemoryConfigurationSource_WithGivenSourceReference()
        {
            var builder = new ConfigBuilderStub();

            TestProxy.AddInMemoryConfigurationProxy(builder, new Dictionary<string, string>());

            Assert.Contains(builder.Sources, (s) => { return s is InMemoryConfigurationSource; });
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
