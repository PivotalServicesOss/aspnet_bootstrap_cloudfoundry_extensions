using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public sealed class AppConfig
    {
        private AppConfig() { }
        static IHost host;

        internal static void Configure(List<Action<HostBuilderContext, IConfigurationBuilder>> configureAppConfigurationDelegates,
                                     List<Action<HostBuilderContext, IServiceCollection>> configureServicesDelegates,
                                     List<Action<HostBuilderContext, ILoggingBuilder>> configureLoggingDelegates,
                                     Action<IServiceCollection> configureIoCDelegate,
                                     Dictionary<string, string> inMemoryConfigurationStore = null)
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    configBuilder.AddInMemoryConfiguration();

                    foreach (var configureAppConfigurationDelegate in configureAppConfigurationDelegates)
                    {
                        configureAppConfigurationDelegate?.Invoke(builderContext, configBuilder);
                    }
                })
                .ConfigureServices((builderContext, services) =>
                {
                    services.AddOptions();

                    foreach (var configureServicesDelegate in configureServicesDelegates)
                    {
                        configureServicesDelegate?.Invoke(builderContext, services);
                    }

                    configureIoCDelegate?.Invoke(services);
                })
                .ConfigureLogging((builder, logBuilder) =>
                {
                    foreach (var configureLoggingDelegate in configureLoggingDelegates)
                    {
                        configureLoggingDelegate?.Invoke(builder, logBuilder);
                    }
                })
                .Build();
        }

        public static T GetService<T>()
        {
            return host.Services.GetService<T>();
        }

        public static IServiceProvider ServiceProvider
        {
            get{ return host.Services; }
        }
    }
}
