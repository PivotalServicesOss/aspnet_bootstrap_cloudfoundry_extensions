using Microsoft.Extensions.Configuration;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration
{
    public static class ConfigurationBuilderExtensions
    {
        internal static IConfigurationBuilder AddWebConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new WebConfigurationSource());
            return builder;
        }
    }
}
