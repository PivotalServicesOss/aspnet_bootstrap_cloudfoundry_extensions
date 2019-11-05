using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration;
using System.Collections.Generic;
using Xunit;

namespace Pcf.Replat.Bootstrap.Base.Tests
{
    public class InMemoryConfigurationTests
    {
        [Fact]
        public void Test_LoadConfigurationFromInMemoryConfiguration()
        {
            var store = new Dictionary<string, string>
            {
                {"key1", "value1" },
                {"key1:subkey1", "value11" },
            };

            var provider = new InMemoryConfigurationProvider(store);
            provider.Load();

            Assert.True(provider.TryGet("key1", out string value));
            Assert.Equal("value1", value);

            Assert.True(provider.TryGet("key1:subkey1", out string value1));
            Assert.Equal("value11", value1);
        }
    }
}
