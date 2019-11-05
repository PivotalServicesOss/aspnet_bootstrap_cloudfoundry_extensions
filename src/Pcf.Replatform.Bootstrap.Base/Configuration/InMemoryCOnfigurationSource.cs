using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration
{
    public class InMemoryConfigurationSource : IConfigurationSource
    {
        internal Dictionary<string, string> Store { get; set; } = new Dictionary<string, string>();

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new InMemoryConfigurationProvider(Store);
        }
    }
}
