using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.AspNet.Bootstrap.Extensions.Reflection;
using Steeltoe.CloudFoundry.Connector.Redis;
using System;
using System.Collections.Generic;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session.Helpers;

namespace PivotalServices.AspNet.Bootstrap.Extensions
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

            instance.AddDefaultConfigurations();

            return instance;
        }
    }
}
