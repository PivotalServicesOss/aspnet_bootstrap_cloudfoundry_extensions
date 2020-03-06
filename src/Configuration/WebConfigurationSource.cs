using Microsoft.Extensions.Configuration;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration
{
    public class WebConfigurationSource : IConfigurationSource
    {
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new WebConfigurationProvider();
        }
    }
}
