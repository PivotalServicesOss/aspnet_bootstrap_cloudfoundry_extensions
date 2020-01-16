using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Health.Contributor;
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

            #region Obsolete and to be removed after the fix for https://github.com/SteeltoeOSS/steeltoe/issues/161
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

            if (loggerFactory == null)
            {
                var serviceProvider = new ServiceCollection()
                        .AddLogging(builder => builder
                            .AddDynamicConsole())
                        .BuildServiceProvider();

                dynamicLoggerProvider = serviceProvider.GetRequiredService<IDynamicLoggerProvider>();
                return serviceProvider.GetRequiredService<ILoggerFactory>();
            }

            dynamicLoggerProvider = DependencyContainer.GetService<IDynamicLoggerProvider>(false);

            if (dynamicLoggerProvider == null)
            {
                dynamicLoggerProvider = GetDynamicLoggerProvider(configuration);
                loggerFactory.AddProvider(dynamicLoggerProvider);
            }

            return loggerFactory;
        }

        private IDynamicLoggerProvider GetDynamicLoggerProvider(IConfiguration configuration)
        {
            var serviceProvider = new ServiceCollection()
                    .AddLogging(builder => builder
                            .AddConfiguration(configuration.GetSection("Logging"))
                            .AddDynamicConsole()
                            .AddFilter<DynamicConsoleLoggerProvider>(null, LogLevel.Trace))
                        .BuildServiceProvider();

            return serviceProvider.GetRequiredService<IDynamicLoggerProvider>();
        }
    }
}
