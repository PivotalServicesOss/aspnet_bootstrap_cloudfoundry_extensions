using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing
{
    /// <summary>
    /// Only for Unit testing purposes
    /// </summary>
    public class TestProxy
    {
        public static void AddInMemoryConfiguration(IConfigurationBuilder builder)
        {
            builder.AddInMemoryConfiguration();
        }

        public static void AddInMemoryConfiguration(IConfigurationBuilder builder, Dictionary<string, string> configStore)
        {
            builder.AddInMemoryConfiguration(configStore);
        }
    }
}
