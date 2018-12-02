using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Replatform.Bootstrap
{
    public class CoreServiceConfig
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
                })
                .ConfigureServices((builderContext, services) =>
                {
                    services.AddOptions();
                    services.AddLogging();
                })


        }

        public static T GetService<T>()
        {
            return host.Services.GetService<T>();
        }

        private static void AddDefaultConfigurationSources(IConfigurationBuilder configBuilder, string environment)
        {
            throw new NotImplementedException();
        }

        private static string GetEnvironment()
        {
            if (Environment.GetEnvironmentVariable(ASPNET_ENV_VAR) == null)
                Environment.SetEnvironmentVariable(ASPNET_ENV_VAR, "Development");

            return Environment.GetEnvironmentVariable(ASPNET_ENV_VAR);
        }
    }
}
