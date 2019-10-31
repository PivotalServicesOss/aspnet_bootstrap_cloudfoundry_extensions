using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using Xunit;

namespace PCF.Replat.Bootstra.Configuration.Tests
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_AddDefaultConfigurationProviders()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureAppConfigurationDelegatesProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();

            AppBuilder.Instance.AddDefaultConfigurationProviders();

            Assert.Single(TestProxy.ConfigureAppConfigurationDelegatesProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
        }

        [Fact]
        public void Test_AddConfigServer()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureAppConfigurationDelegatesProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();

            AppBuilder.Instance.AddConfigServer();

            Assert.Single(TestProxy.ConfigureAppConfigurationDelegatesProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);

            Assert.Equal("${vcap.application.name}", TestProxy.InMemoryConfigStoreProxy["spring:application:name"]);
            Assert.Equal("${vcap.application.name}", TestProxy.InMemoryConfigStoreProxy["spring:cloud:config:name"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["spring:cloud:config:validate_certificates"]);
            Assert.Equal("true", TestProxy.InMemoryConfigStoreProxy["spring:cloud:config:failFast"]);
        }
    }
}
