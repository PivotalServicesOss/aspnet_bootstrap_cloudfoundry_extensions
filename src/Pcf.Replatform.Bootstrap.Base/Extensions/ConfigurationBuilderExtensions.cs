using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class ConfigurationBuilderExtensions
    {
        internal static IConfigurationBuilder AddInMemoryConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new InMemoryConfigurationSource());
            return builder;
        }

        internal static IConfigurationBuilder AddInMemoryConfiguration(this IConfigurationBuilder builder, Dictionary<string, string> configStore)
        {
            builder.Add(new InMemoryConfigurationSource() { Store = configStore });
            return builder;
        }
    }
}
