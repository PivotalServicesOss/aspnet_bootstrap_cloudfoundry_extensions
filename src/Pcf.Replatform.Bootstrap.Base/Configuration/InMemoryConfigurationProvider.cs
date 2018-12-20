using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration
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
                Data.Add(item.Key, item.Value);
            }
        }
    }
}
