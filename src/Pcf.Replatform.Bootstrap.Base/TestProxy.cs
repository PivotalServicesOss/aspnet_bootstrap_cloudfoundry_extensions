using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing
{
    [Obsolete("Not for production use, only for internal testing purpose")]
    public class TestProxy
    {
        public static void AddInMemoryConfigurationProxy(IConfigurationBuilder builder)
        {
            builder.AddInMemoryConfiguration();
        }

        public static void AddInMemoryConfigurationProxy(IConfigurationBuilder builder, Dictionary<string, string> configStore)
        {
            builder.AddInMemoryConfiguration(configStore);
        }

        public static List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesDelegatesProxy { get; } = AppBuilder.Instance.ConfigureServicesDelegates;
        public static List<Action<HostBuilderContext, IConfigurationBuilder>> ConfigureAppConfigurationDelegatesProxy { get; } = AppBuilder.Instance.ConfigureAppConfigurationDelegates;
        public static List<Action<HostBuilderContext, ILoggingBuilder>> ConfigureLoggingDelegatesProxy { get; } = AppBuilder.Instance.ConfigureLoggingDelegates;
        public static List<IActuator> ActuatorsProxy { get; } = AppBuilder.Instance.Actuators;
        public static Dictionary<string, string> InMemoryConfigStoreProxy { get; } = AppBuilder.Instance.InMemoryConfigStore;
        public static List<Type> HandlersProxy { get; } = AppBuilder.Instance.Handlers;
    }
}
