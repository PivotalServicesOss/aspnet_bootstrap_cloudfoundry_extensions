using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.CloudFoundry;
using Steeltoe.Management.Endpoint.Handler;
using Steeltoe.Management.Endpoint.Health;
using Steeltoe.Management.Endpoint.Health.Contributor;
using Steeltoe.Management.Endpoint.Hypermedia;
using Steeltoe.Management.Endpoint.Info;
using Steeltoe.Management.Endpoint.Info.Contributor;
using Steeltoe.Management.Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
{
    [Obsolete("Once the issue https://github.com/SteeltoeOSS/steeltoe/issues/161 is fixed, this method will be removed, instead steeltoe's will be used")]
    internal static class ActuatorConfiguratorOverrides
    {
        public static void UseHypermediaActuator(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            var _mgmtOptions = ReflectionHelper.GetNonPublicStaticFieldValue<List<IManagementOptions>>(typeof(ActuatorConfigurator), "_mgmtOptions");
            var securityServices = ReflectionHelper.GetNonPublicStaticPropertyValue<List<ISecurityService>>(typeof(ActuatorConfigurator), "SecurityServices");

            HypermediaEndpointOptions hypermediaEndpointOptions = new HypermediaEndpointOptions(configuration);
            ActuatorManagementOptions actuatorManagementOptions = _mgmtOptions.OfType<ActuatorManagementOptions>().SingleOrDefault();
            if (actuatorManagementOptions == null)
            {
                actuatorManagementOptions = new ActuatorManagementOptions(configuration);
                _mgmtOptions.Add(actuatorManagementOptions);
            }
            actuatorManagementOptions.EndpointOptions.Add(hypermediaEndpointOptions);
            ActuatorHypermediaHandler item = new ActuatorHypermediaOpenAccessHandler(new ActuatorEndpoint(hypermediaEndpointOptions, _mgmtOptions, loggerFactory.CreateLogger<ActuatorEndpoint>()), securityServices, _mgmtOptions, loggerFactory.CreateLogger<ActuatorHypermediaHandler>());
            ActuatorConfigurator.ConfiguredHandlers.Add(item);
            if (!ActuatorConfigurator.ConfiguredHandlers.OfType<CloudFoundryCorsHandler>().Any())
            {
                CloudFoundryCorsHandler item2 = new CloudFoundryCorsHandler(hypermediaEndpointOptions, securityServices, _mgmtOptions, loggerFactory.CreateLogger<CloudFoundryCorsHandler>());
                ActuatorConfigurator.ConfiguredHandlers.Add(item2);
            }
        }

        public static void UseHealthActuator(IConfiguration configuration, IHealthAggregator healthAggregator = null, IEnumerable<IHealthContributor> contributors = null, ILoggerFactory loggerFactory = null)
        {
            var _mgmtOptions = ReflectionHelper.GetNonPublicStaticFieldValue<List<IManagementOptions>>(typeof(ActuatorConfigurator), "_mgmtOptions");
            var securityServices = ReflectionHelper.GetNonPublicStaticPropertyValue<List<ISecurityService>>(typeof(ActuatorConfigurator), "SecurityServices");

            HealthEndpointOptions options = new HealthEndpointOptions(configuration);
            _mgmtOptions.RegisterEndpointOptions(configuration, options);

            if (!ActuatorConfigurator.ConfiguredHandlers.OfType<HealthHandler>().Any())
            {
                healthAggregator = (healthAggregator ?? new DefaultHealthAggregator());
                contributors = (contributors ?? new List<IHealthContributor>
            {
                (IHealthContributor)(object)new DiskSpaceContributor(new DiskSpaceContributorOptions(configuration))
            });
                HealthHandler item = new HealthOpenAccessHandler(new HealthEndpoint(options, healthAggregator, contributors, loggerFactory.CreateLogger<HealthEndpoint>()), securityServices, _mgmtOptions, loggerFactory.CreateLogger<HealthHandler>());
                ActuatorConfigurator.ConfiguredHandlers.Add(item);
            }
        }

        public static void UseInfoActuator(IConfiguration configuration, IEnumerable<IInfoContributor> contributors = null, ILoggerFactory loggerFactory = null)
        {
            var _mgmtOptions = ReflectionHelper.GetNonPublicStaticFieldValue<List<IManagementOptions>>(typeof(ActuatorConfigurator), "_mgmtOptions");
            var securityServices = ReflectionHelper.GetNonPublicStaticPropertyValue<List<ISecurityService>>(typeof(ActuatorConfigurator), "SecurityServices");

            InfoEndpointOptions options = new InfoEndpointOptions(configuration);
            _mgmtOptions.RegisterEndpointOptions(configuration, options);

            if (!ActuatorConfigurator.ConfiguredHandlers.OfType<InfoHandler>().Any())
            {
                contributors = (contributors ?? new List<IInfoContributor>
            {
                new GitInfoContributor(),
                new AppSettingsInfoContributor(configuration)
            });
                InfoHandler item = new InfoOpenAccessHandler(new InfoEndpoint(options, contributors, loggerFactory.CreateLogger<InfoEndpoint>()), securityServices, _mgmtOptions, loggerFactory.CreateLogger<InfoHandler>());
                ActuatorConfigurator.ConfiguredHandlers.Add(item);
            }
        }

        private static void RegisterEndpointOptions(this List<IManagementOptions> mgmtOptions, IConfiguration configuration, IEndpointOptions options)
        {
            if (!mgmtOptions.Any())
            {
                mgmtOptions.Add(new CloudFoundryManagementOptions(configuration));
                mgmtOptions.Add(new ActuatorManagementOptions(configuration));
            }
            foreach (IManagementOptions mgmtOption in mgmtOptions)
            {
                if (!mgmtOption.EndpointOptions.Contains(options))
                {
                    mgmtOption.EndpointOptions.Add(options);
                }
            }
        }
    }
}