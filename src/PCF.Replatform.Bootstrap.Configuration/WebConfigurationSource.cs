using Microsoft.Extensions.Configuration;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration
{
    public class WebConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new WebConfigurationProvider();
        }
    }
}
