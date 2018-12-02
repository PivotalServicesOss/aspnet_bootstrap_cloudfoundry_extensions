using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions;
using Serilog;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base
{
    internal class AppConfig
    {
        const string ASPNET_ENV_VAR = "ASPNET_ENVIRONMENT";
        static IHost host;

        public static void Configure(Action<HostBuilderContext, IConfigurationBuilder> configureAppConfigurationDelegate,
                                     Action<HostBuilderContext, IServiceCollection> configureServicesDelegate,
                                     Action<ILoggingBuilder> configureLoggingDelegate)
        {
            host = new HostBuilder()
                .ConfigureAppConfiguration((builderContext, configBuilder) =>
                {
                    AddDefaultConfigurationSources(configBuilder, GetEnvironment());

                    configureAppConfigurationDelegate?.Invoke(builderContext, configBuilder);
                })
                .ConfigureServices((builderContext, services) =>
                {
                    InitializeSerilog(builderContext.Configuration);

                    services.AddOptions();
                    services.AddLogging((builder)=> 
                    {
                        builder.AddSerilog(dispose: true);
                    });
                    services.AddDistributedTracing(builderContext.Configuration);
                    services.ConfigureCloudFoundryOptions(builderContext.Configuration);

                    configureServicesDelegate?.Invoke(builderContext, services);

                    OverrideWebConfiguration(builderContext.Configuration);
                })
                .ConfigureLogging((builder)=>
                {
                    configureLoggingDelegate?.Invoke(builder);
                })
                .Build();
        }

        public static T GetService<T>()
        {
            return host.Services.GetService<T>();
        }

        private static void AddDefaultConfigurationSources(IConfigurationBuilder configBuilder, string environment)
        {
            configBuilder.AddWebConfiguration();
            configBuilder.AddJsonFile("appSettings.json", false, false);
            configBuilder.AddJsonFile($"appSettings.{environment}.json", true, false);
            configBuilder.AddEnvironmentVariables();
        }

        private static void OverrideWebConfiguration(IConfiguration configuration)
        {
            var webConfiguration = WebConfigurationManager.OpenWebConfiguration("~");

            var existingAppSettingKeys = webConfiguration.AppSettings.Settings.AllKeys.ToList();
            var existingConnectionStrings = webConfiguration.ConnectionStrings.ConnectionStrings
                                            .Cast<ConnectionStringSettings>().ToDictionary(v => v.Name, v => v);

            OverrideAppSettingsSection(configuration, webConfiguration, existingAppSettingKeys);
            OverrideConnectionStringSection(configuration, webConfiguration, existingConnectionStrings);
            SaveAndResetWebConfiguration(webConfiguration);
        }

        private static void SaveAndResetWebConfiguration(System.Configuration.Configuration webConfiguration)
        {
            webConfiguration.Save();
            typeof(ConfigurationManager).GetField("s_initState", BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, 0);
        }

        private static void OverrideConnectionStringSection(IConfiguration configuration, System.Configuration.Configuration webConfiguration, Dictionary<string, ConnectionStringSettings> existingConnectionStrings)
        {
            var connectionStrings = configuration.GetSection("ConnectionStrings").GetChildren().ToDictionary(c => c.Key, c => c.Value);
            var providerNames = configuration.GetSection("Providers").GetChildren().ToDictionary(c => c.Key, c => c.Value);

            foreach (var item in connectionStrings)
            {
                if (!connectionStrings.Any(s => s.Key == item.Key))
                    continue;

                webConfiguration.ConnectionStrings.ConnectionStrings[item.Key].ConnectionString = connectionStrings[item.Key];
                webConfiguration.ConnectionStrings.ConnectionStrings[item.Key].ProviderName = providerNames[item.Key];
            }
        }

        private static void OverrideAppSettingsSection(IConfiguration configuration, System.Configuration.Configuration webConfiguration, List<string> existingAppSettingKeys)
        {
            var appSettings = configuration.GetSection("AppSettings").GetChildren().ToDictionary(c => c.Key, c => c.Value);

            foreach (var item in existingAppSettingKeys)
            {
                if (!appSettings.Any(s => s.Key == item))
                    continue;

                webConfiguration.AppSettings.Settings[item].Value = appSettings[item];
            }
        }

        private static void InitializeSerilog(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
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
