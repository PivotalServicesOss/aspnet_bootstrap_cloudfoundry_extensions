using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddWebConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new WebConfigurationSource());
            return builder;
        }

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
