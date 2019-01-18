using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AppBuilder
    {
        private AppBuilder() { }

        private static Action<HostBuilderContext, IConfigurationBuilder> configureAppConfiguration;
        private static Action<HostBuilderContext, IServiceCollection> configureServices;
        private static Action<HostBuilderContext, ILoggingBuilder> configureLogging;
        private static Action<IServiceCollection> configureIoC;
        private static bool persistSessionToRedis;
        private static bool addRedisDistributedCache;
        private static bool addConfigServer;
        private static bool useCloudFoundryActuators;
        private static bool useCloudFoundryMetricsActuator;
        private static bool useCloudFoundryMerticsForwarder;
        private static IMetricsExporter metricsExporter;
        private static bool useCustomIoC;
        private static Func<IDependencyResolver> mvcDependencyResolverFunc;
        private static Func<System.Web.Http.Dependencies.IDependencyResolver> webApiDependencyResolverFunc;

        public static readonly AppBuilder Instance = new AppBuilder();

        public Dictionary<string, string> InMemoryConfigStore { get; } = new Dictionary<string, string>();

        public AppBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            configureAppConfiguration = configureDelegate;
            return Instance;
        }

        public AppBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            configureServices = configureDelegate;
            return Instance;
        }

        public AppBuilder ConfigureLogging(Action<HostBuilderContext, ILoggingBuilder> configureDelegate)
        {
            configureLogging = configureDelegate;
            return Instance;
        }

        public AppBuilder PersistSessionToRedis()
        {
            persistSessionToRedis = true;
            return Instance;
        }

        public AppBuilder AddRedisDistributedCache()
        {
            addRedisDistributedCache = true;
            return Instance;
        }

        public AppBuilder AddConfigServer()
        {
            addConfigServer = true;
            return Instance;
        }

        public AppBuilder UseCloudFoundryMerticsForwarder()
        {
            useCloudFoundryMerticsForwarder = true;
            return Instance;
        }

        public AppBuilder UseCloudFoundryActuators(bool includeMetricsActuator = false)
        {
            useCloudFoundryActuators = true;
            useCloudFoundryMetricsActuator = includeMetricsActuator;
            return Instance;
        }

        public AppBuilder ConfigureIoC(Func<System.Web.Http.Dependencies.IDependencyResolver> webApiResolverFunc, Func<IDependencyResolver> mvcResolverFunc, Action<IServiceCollection> configureServiceProvider)
        {
            useCustomIoC = true;
            webApiDependencyResolverFunc = webApiResolverFunc;
            mvcDependencyResolverFunc = mvcResolverFunc;
            configureIoC = configureServiceProvider;
            return Instance;
        }

        public AppBuilder Build()
        {
            AppConfig.Configure(configureAppConfiguration,
                                configureServices,
                                configureLogging,
                                configureIoC,
                                persistSessionToRedis,
                                addRedisDistributedCache,
                                addConfigServer,
                                InMemoryConfigStore);

            if (useCloudFoundryActuators)
                ConfigureActuators();

            if (useCloudFoundryMerticsForwarder)
                ConfigureMetricsForwarder();

            if (!useCustomIoC)
                InstallDefaultDependencyResolvers();
            else
                InstallCustomDependencyResolvers();

            return Instance;
        }

        public void Start()
        {
            if (useCloudFoundryActuators)
                DiagnosticsManager.Instance.Start();

            if (useCloudFoundryMerticsForwarder)
                metricsExporter?.Start();
        }

        public void Stop()
        {
            if (useCloudFoundryActuators)
                DiagnosticsManager.Instance.Stop();

            if (useCloudFoundryMerticsForwarder)
                metricsExporter?.Stop();
        }

        private void InstallDefaultDependencyResolvers()
        {
            var resolver = new DefaultDependencyResolver(AppConfig.ServiceProvider);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            DependencyResolver.SetResolver(resolver);
        }

        private void InstallCustomDependencyResolvers()
        {
            GlobalConfiguration.Configuration.DependencyResolver = webApiDependencyResolverFunc?.Invoke();
            DependencyResolver.SetResolver(mvcDependencyResolverFunc?.Invoke());
        }

        private void ConfigureMetricsForwarder()
        {
            var configuration = AppConfig.GetService<IConfiguration>();
            var loggerFactory = AppConfig.GetService<ILoggerFactory>();

            metricsExporter = new CloudFoundryForwarderExporter(new CloudFoundryForwarderOptions(configuration),
                                                                OpenCensusStats.Instance,
                                                                loggerFactory.CreateLogger<CloudFoundryForwarderExporter>());
        }

        private void ConfigureActuators()
        {
            var configuration = AppConfig.GetService<IConfiguration>();
            var loggerFactory = AppConfig.GetService<ILoggerFactory>();
            var dynamicLoggerProvider = new DynamicLoggerProvider(new ConsoleLoggerSettings().FromConfiguration(configuration));
            loggerFactory.AddProvider(dynamicLoggerProvider);

            ActuatorConfigurator.UseCloudFoundryActuators(configuration,
                                                            dynamicLoggerProvider,
                                                            GetHealthContributors(),
                                                            GlobalConfiguration.Configuration.Services.GetApiExplorer(),
                                                            loggerFactory);

            if (useCloudFoundryMetricsActuator)
                ActuatorConfigurator.UseMetricsActuator(configuration, loggerFactory);
        }

        private IEnumerable<IHealthContributor> GetHealthContributors()
        {
            var healthContributors = AppConfig.GetService<IEnumerable<IHealthContributor>>().ToList();
            healthContributors.Add(new DiskSpaceContributor());
            return healthContributors;
        }
    }
}
