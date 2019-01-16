using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Helpers;
using Pivotal.Extensions.Configuration.ConfigServer;
using Serilog;
using Steeltoe.CloudFoundry.Connector.Redis;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AppConfig
    {
        const string ASPNET_ENV_VAR = "ASPNET_ENVIRONMENT";
        static IHost host;

        public static void Configure(Action<HostBuilderContext, IConfigurationBuilder> configureAppConfigurationDelegate,
                                     Action<HostBuilderContext, IServiceCollection> configureServicesDelegate,
                                     Action<ILoggingBuilder> configureLoggingDelegate,
                                     bool persistSessionToRedis,
                                     bool addRedisDistributedCache,
                                     bool addConfigServer,
                                     Dictionary<string, string> inMemoryConfigurationStore = null)
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    AddDefaultConfigurationSources(configBuilder, GetEnvironment(), inMemoryConfigurationStore, addConfigServer);

                    configureAppConfigurationDelegate?.Invoke(builderContext, configBuilder);
                })
                .ConfigureServices((builderContext, services) =>
                {
                    services.AddOptions();

                    InitializeSerilog(builderContext.Configuration);

                    services.AddLogging((builder) =>
                    {
                        builder.AddSerilog(dispose: true);
                    });

                    services.ConfigureCloudFoundryOptions(builderContext.Configuration);

                    if (persistSessionToRedis)
                    {
                        WebConfigurationHelper.ValidateWebConfigurationForRedisSessionState();
                        services.AddRedisConnectionMultiplexer(builderContext.Configuration);
                    }

                    services.AddDefaultDiagnosticsDependencies(builderContext.Configuration);

                    configureServicesDelegate?.Invoke(builderContext, services);

                    WebConfigurationHelper.OverrideWebConfiguration(builderContext.Configuration);
                })
                .ConfigureLogging((builder) =>
                {
                    configureLoggingDelegate?.Invoke(builder);
                })
                .Build();
        }

        public static T GetService<T>()
        {
            return host.Services.GetService<T>();
        }

        private static void AddDefaultConfigurationSources(IConfigurationBuilder configBuilder, string environment,
                                                            Dictionary<string, string> inMemoryConfigurationStore,
                                                            bool addConfigServer)
        {
            configBuilder.AddWebConfiguration();
            configBuilder.AddJsonFile("appSettings.json", false, false);
            configBuilder.AddJsonFile($"appSettings.{environment?.ToLower()}.json", true, false);
            configBuilder.AddEnvironmentVariables();
            configBuilder.AddCloudFoundry();

            if (addConfigServer)
            {
                var clientSettings = new ConfigServerClientSettings { Environment = environment };
                configBuilder.AddConfigServer();
            }

            if (inMemoryConfigurationStore == null)
                configBuilder.AddInMemoryConfiguration();
            else
                configBuilder.AddInMemoryConfiguration(inMemoryConfigurationStore);

        }

        private static void InitializeSerilog(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .Filter.ByExcluding("Contains(@Message, 'cloudfoundryapplication')")
                .CreateLogger();
        }

        private static string GetEnvironment()
        {
            if (Environment.GetEnvironmentVariable(ASPNET_ENV_VAR) == null)
                Environment.SetEnvironmentVariable(ASPNET_ENV_VAR, "Development");

            return Environment.GetEnvironmentVariable(ASPNET_ENV_VAR);
        }
    }
}
