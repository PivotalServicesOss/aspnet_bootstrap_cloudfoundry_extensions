using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using System.Web.SessionState;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Helpers
{
    internal class WebConfigurationHelper
    {
        const string CONN_STRING_SECTION = "ConnectionStrings";
        const string PROVIDERS_SECTION = "Providers";
        const string APP_SETTINGS_SECTION = "AppSettings";
        const string SESSION_STATE_SECTION = "system.web/sessionState";
        const string MACHINE_KEY_SECTION = "system.web/machineKey";
        const string REDIS_SESSION_STATE_STORE_NAME = "RedisSessionStateStore";

        public static void OverrideWebConfiguration(IConfiguration configuration)
        {
            var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");

            OverrideAppSettingsSection(configuration, webConfiguration);
            OverrideConnectionStringSection(configuration, webConfiguration);
        }

        public static void ValidateWebConfigurationForRedisSessionState()
        {
            var sessionSection = (SessionStateSection)ConfigurationManager.GetSection(SESSION_STATE_SECTION);

            if (sessionSection.Mode != SessionStateMode.Custom || sessionSection.CustomProvider != REDIS_SESSION_STATE_STORE_NAME)
                throw new ConfigurationErrorsException($"Missing 'Custom' sessionState section with provider name '{REDIS_SESSION_STATE_STORE_NAME}'");

            if(sessionSection.Providers == null || sessionSection.Providers.Count == 0)
                throw new ConfigurationErrorsException($"Missing session state providers");

            if (sessionSection.Providers[REDIS_SESSION_STATE_STORE_NAME] == null)
                throw new ConfigurationErrorsException($"Missing session state provider '{REDIS_SESSION_STATE_STORE_NAME}'");

            var machineKeySection = (MachineKeySection)ConfigurationManager.GetSection(MACHINE_KEY_SECTION);

            if (machineKeySection == null)
                throw new ConfigurationErrorsException($"Missing machineKey section");
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
