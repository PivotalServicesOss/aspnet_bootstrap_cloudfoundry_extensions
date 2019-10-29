using Microsoft.Extensions.Configuration;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddWebConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new WebConfigurationSource());
            return builder;
        }
    }
}
