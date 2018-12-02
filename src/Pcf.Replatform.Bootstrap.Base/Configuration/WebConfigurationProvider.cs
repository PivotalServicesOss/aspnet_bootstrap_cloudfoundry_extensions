using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Configuration
{
    public class WebConfigurationProvider : ConfigurationProvider
    {
        const string CON_STR_PREFIX = "ConnectionStrings";
        const string PROVIDER_PREFIX = "Providers";
        const string APP_SETT_PREFIX = "AppSettings";

        public override void Load()
        {
            Load(ConfigurationManager.AppSettings);
            Load(ConfigurationManager.ConnectionStrings);
        }

        private void Load(NameValueCollection appSettings)
        {
            foreach (var settingKey in appSettings.AllKeys)
            {
                var key = $"{APP_SETT_PREFIX}:{settingKey}";
                Data[key] = appSettings[settingKey];
            }
        }

        private void Load(ConnectionStringSettingsCollection connectionStrings)
        {
            var connectionStringSettings = from c in connectionStrings.Cast<ConnectionStringSettings>() select c;

            foreach (var setting in connectionStringSettings)
            {
                var key = $"{CON_STR_PREFIX}:{setting.Name}";
                Data[key] = setting.ConnectionString;

                key = $"{PROVIDER_PREFIX}:{setting.Name}";
                Data[key] = setting.ProviderName;
            }
        }
    }
}
