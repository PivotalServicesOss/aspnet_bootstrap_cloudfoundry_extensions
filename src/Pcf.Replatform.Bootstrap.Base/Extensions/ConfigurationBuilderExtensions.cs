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
    }
}
