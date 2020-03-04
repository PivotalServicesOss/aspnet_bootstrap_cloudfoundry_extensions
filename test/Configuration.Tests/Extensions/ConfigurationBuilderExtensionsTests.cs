using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration.Testing;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Test.Helpers;
using Xunit;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration.Tests
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
