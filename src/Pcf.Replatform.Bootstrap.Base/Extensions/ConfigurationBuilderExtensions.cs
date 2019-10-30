using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    internal static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddInMemoryConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new InMemoryConfigurationSource());
            return builder;
        }

        public static IConfigurationBuilder AddInMemoryConfiguration(this IConfigurationBuilder builder, Dictionary<string, string> configStore)
        {
            builder.Add(new InMemoryConfigurationSource() { Store = configStore });
            return builder;
        }
    }
}
