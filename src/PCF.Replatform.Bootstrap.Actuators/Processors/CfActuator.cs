using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Handler;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.Endpoint.Hypermedia;
using Steeltoe.Management.Endpoint.Security;
using Steeltoe.Management.Hypermedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
{
    public class CfActuator : IActuator
    {
        IDynamicLoggerProvider dynamicLoggerProvider = null;

        public void Configure()
        {
            var configuration = DependencyContainer.GetService<IConfiguration>();
            var loggerFactory = GetLoggerFactory(configuration);
            loggerFactory.AddProvider(dynamicLoggerProvider);

            #region Obsolete nd to be removed after the fix for https://github.com/SteeltoeOSS/steeltoe/issues/161
            ActuatorConfiguratorOverrides.UseHypermediaActuator(configuration, loggerFactory);
            ActuatorConfigurator.UseCloudFoundrySecurity(configuration, null, loggerFactory);
            ActuatorConfigurator.UseCloudFoundryActuator(configuration, loggerFactory);
            ActuatorConfiguratorOverrides.UseHealthActuator(configuration, null, GetHealthContributors(), loggerFactory);
            ActuatorConfigurator.UseHeapDumpActuator(configuration, null, loggerFactory);
            ActuatorConfigurator.UseThreadDumpActuator(configuration, MediaTypeVersion.V1, null, loggerFactory);
            ActuatorConfiguratorOverrides.UseInfoActuator(configuration, null, loggerFactory);
            ActuatorConfigurator.UseLoggerActuator(configuration, dynamicLoggerProvider, loggerFactory);
            ActuatorConfigurator.UseTraceActuator(configuration, MediaTypeVersion.V1, null, loggerFactory);
            ActuatorConfigurator.UseMappingsActuator(configuration, GlobalConfiguration.Configuration.Services.GetApiExplorer(), loggerFactory);
            #endregion

            #region Uncoment after the fix for https://github.com/SteeltoeOSS/steeltoe/issues/161
            //ActuatorConfigurator.UseCloudFoundryActuators(configuration,
            //                                                dynamicLoggerProvider,
            //                                                MediaTypeVersion.V1,
            //                                                ActuatorContext.ActuatorAndCloudFoundry,
            //                                                GetHealthContributors(),
            //                                                GlobalConfiguration.Configuration.Services.GetApiExplorer(),
            //                                                loggerFactory);
            #endregion

            ActuatorConfigurator.UseMetricsActuator(configuration, loggerFactory);
        }

        public void Start()
        {
            DiagnosticsManager.Instance.Start();
        }

        public void Stop()
        {
            DiagnosticsManager.Instance.Stop();
        }

        private IEnumerable<IHealthContributor> GetHealthContributors()
        {
            var healthContributors = DependencyContainer.GetService<IEnumerable<IHealthContributor>>().ToList();
            healthContributors.Add(new DiskSpaceContributor());
            return healthContributors;
        }

        private ILoggerFactory GetLoggerFactory(IConfiguration configuration)
        {
            var loggerFactory = DependencyContainer.GetService<ILoggerFactory>(false);

            dynamicLoggerProvider = DependencyContainer.GetService<IDynamicLoggerProvider>(false)
                    ?? GetDynamicLoggerProvider(configuration);

            if (loggerFactory == null)
                loggerFactory.AddProvider(dynamicLoggerProvider);

            return loggerFactory;
        }


        private IDynamicLoggerProvider GetDynamicLoggerProvider(IConfiguration configuration)
        {
            var serviceProvider = new ServiceCollection()
                        .AddLogging(builder => builder
                            .AddConfiguration(configuration.GetSection("Logging"))
                            .AddDynamicConsole()
                            .AddFilter<DynamicConsoleLoggerProvider>(null, LogLevel.Information))
                        .BuildServiceProvider();

            var loggerProviderConfiguration = serviceProvider.GetService<ILoggerProviderConfiguration<ConsoleLoggerProvider>>();
            return serviceProvider.GetRequiredService<IDynamicLoggerProvider>();
        }
    }
}
