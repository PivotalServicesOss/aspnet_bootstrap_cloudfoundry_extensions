using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using Steeltoe.CloudFoundry.Connector.Redis;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder PersistSessionToRedis(this AppBuilder instance)
        {
            instance.ConfigureServicesDelegates.Add((builderContext, services)=> {
                WebConfigurationHelper.ValidateWebConfigurationForRedisSessionState();
                services.AddRedisConnectionMultiplexer(builderContext.Configuration);
            });
            return instance;
        }
    }
}
