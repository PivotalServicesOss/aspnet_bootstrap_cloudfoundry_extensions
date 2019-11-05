using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Configuration
{
    public class InMemoryConfigurationProvider : ConfigurationProvider
    {
        private Dictionary<string, string> store;

        public InMemoryConfigurationProvider(Dictionary<string, string> store)
        {
            this.store = store;
        }

        public override void Load()
        {
            foreach (var item in store)
            {
                Data[item.Key] = item.Value;
            }
        }
    }
}
