using Microsoft.Extensions.Configuration;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration.Testing
{
    public class TestProxy
    {
        /// <summary>
        /// Only for testing purpose
        /// </summary>
        public static void AddWebConfiguration(IConfigurationBuilder builder)
        {
            builder.AddWebConfiguration();
        }
    }
}
