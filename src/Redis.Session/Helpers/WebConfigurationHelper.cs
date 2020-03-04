using System.Configuration;
using System.Web.Configuration;
using System.Web.SessionState;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session.Helpers
{
    internal class WebConfigurationHelper
    {
        const string SESSION_STATE_SECTION = "system.web/sessionState";
        const string MACHINE_KEY_SECTION = "system.web/machineKey";
        const string REDIS_SESSION_STATE_STORE_NAME = "RedisSessionStateStore";

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
    }
}
