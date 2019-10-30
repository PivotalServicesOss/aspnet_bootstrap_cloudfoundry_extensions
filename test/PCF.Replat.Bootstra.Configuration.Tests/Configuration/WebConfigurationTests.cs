using PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration;
using Xunit;

namespace PCF.Replat.Bootstra.Configuration.Tests
{
    public class WebConfigurationTests
    {
        [Fact]
        public void Test_AppSettingsFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.True(provider.TryGet("AppSettings:appkey1", out string value));
            Assert.Equal("appvalue1", value);
        }

        [Fact]
        public void Test_ConnectionStringFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.True(provider.TryGet("ConnectionStrings:conn1", out string value));
            Assert.Equal("my dummy connection string", value);
        }

        [Fact]
        public void Test_ProvidersFromConfigFile()
        {
            var provider = new WebConfigurationProvider();
            provider.Load();

            Assert.True(provider.TryGet("Providers:conn1", out string value));
            Assert.Equal("provider1", value);
        }
    }
}
