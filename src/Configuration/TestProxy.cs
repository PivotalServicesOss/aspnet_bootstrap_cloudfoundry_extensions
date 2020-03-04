using Microsoft.Extensions.Configuration;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration.Testing
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
