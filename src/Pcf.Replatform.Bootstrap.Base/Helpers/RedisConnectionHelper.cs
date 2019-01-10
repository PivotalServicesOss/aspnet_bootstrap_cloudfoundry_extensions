using Microsoft.Web.Redis;
using StackExchange.Redis;
using System;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Helpers
{
    public static class RedisConnectionHelper
    {
        static IConnectionMultiplexer connectionMultiplexer;
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
