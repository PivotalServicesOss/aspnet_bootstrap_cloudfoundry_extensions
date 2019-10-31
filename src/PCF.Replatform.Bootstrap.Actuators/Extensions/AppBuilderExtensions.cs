using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder AddHealthActuators(this AppBuilder instance, string basePath = null)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstanceFieldValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            if (string.IsNullOrWhiteSpace(basePath))
                inMemoryConfigStore.Add("management:endpoints:path", "/cloudfoundryapplication");
            else
                inMemoryConfigStore.Add("management:endpoints:path", $"{basePath.TrimEnd('/')}/cloudfoundryapplication");

            inMemoryConfigStore.Add("management:endpoints:cloudfoundry:validateCertificates", "false");

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfActuator());

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                .Add((builderContext, services) => {
                services.AddControllers();
            });
            return instance;
        }

        public static AppBuilder AddMetricsForwarder(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstanceFieldValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore.Add("management:metrics:exporter:cloudfoundry:validateCertificates", "false");

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfMetricsForwarder());

            return instance;
        }
    }
}
