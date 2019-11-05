using PCF.Replatform.Test.Helpers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration.Testing;
using Xunit;

namespace PCF.Replat.Bootstra.Configuration.Tests
{
    public class ConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void Test_If_AddWebConfiguration_Adds_WebConfigurationSource()
        {
            var builder = new ConfigBuilderStub();
            TestProxy.AddWebConfiguration(builder);

            Assert.Contains(builder.Sources, (s) => { return s is WebConfigurationSource; });
        }
    }
}
