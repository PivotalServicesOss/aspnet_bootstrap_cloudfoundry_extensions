using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration
{
    internal class WebConfigurationHelper
    {
        const string CONN_STRING_SECTION = "ConnectionStrings";
        const string PROVIDERS_SECTION = "Providers";
        const string APP_SETTINGS_SECTION = "AppSettings";

        public static void OverrideWebConfiguration(IConfiguration configuration)
        {
            var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");

            OverrideAppSettingsSection(configuration, webConfiguration);
            OverrideConnectionStringSection(configuration, webConfiguration);
        }

        private static void OverrideConnectionStringSection(IConfiguration configuration, System.Configuration.Configuration webConfiguration)
        {
            var connectionStrings = configuration.GetSection(CONN_STRING_SECTION).GetChildren().ToDictionary(c => c.Key, c => c.Value);
            var providerNames = configuration.GetSection(PROVIDERS_SECTION).GetChildren().ToDictionary(c => c.Key, c => c.Value);

            var element = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            var collection = typeof(ConfigurationElementCollection).GetField("bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            collection.SetValue(ConfigurationManager.ConnectionStrings, false);

            foreach (var item in connectionStrings)
            {
                ConfigurationManager.ConnectionStrings.Remove(new ConnectionStringSettings(item.Key, string.Empty));
                ConfigurationManager.ConnectionStrings.Add(new ConnectionStringSettings(item.Key, item.Value, providerNames.TryGetValue(item.Key, out string pName) ? pName : null));
            }

            collection.SetValue(ConfigurationManager.ConnectionStrings, true);
            element.SetValue(ConfigurationManager.ConnectionStrings, true);
        }

        private static void OverrideAppSettingsSection(IConfiguration configuration, System.Configuration.Configuration webConfiguration)
        {
            var appSettings = configuration.GetSection(APP_SETTINGS_SECTION).GetChildren().ToDictionary(c => c.Key, c => c.Value);

            foreach (var item in appSettings)
            {
                ConfigurationManager.AppSettings.Set(item.Key, item.Value);
            }
        }
    }
}
