using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session;
using Steeltoe.CloudFoundry.Connector.Redis;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder PersistSessionToRedis(this AppBuilder instance)
        {
            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                .Add((builderContext, services)=> {
                    WebConfigurationHelper.ValidateWebConfigurationForRedisSessionState();
                    services.AddRedisConnectionMultiplexer(builderContext.Configuration);
            });
            return instance;
        }
    }
}
