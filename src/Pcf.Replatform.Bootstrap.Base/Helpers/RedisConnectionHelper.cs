using Microsoft.Web.Redis;
using StackExchange.Redis;
using System;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Helpers
{
    public static class RedisConnectionHelper
    {
        static IConnectionMultiplexer connectionMultiplexer;

        //Just to protect the reference to Microsoft.Web.RedisSessionStateProvider (avoid accidental removal)
        static RedisSessionStateProvider redisSessionStateProvider;

        static RedisConnectionHelper()
        {
            connectionMultiplexer = AppConfig.GetService<IConnectionMultiplexer>() ?? throw new ArgumentNullException(nameof(IConnectionMultiplexer));
        }

        public static string GetConnectionString()
        {
            return connectionMultiplexer.Configuration;
        }
    }
}
