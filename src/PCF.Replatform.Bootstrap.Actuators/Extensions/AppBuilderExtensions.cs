using PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Enables all CF Actuators for Apps Manager
        /// Only actuator/health and actuator/info endpoints are exposed due to issue https://github.com/SteeltoeOSS/steeltoe/issues/161
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static AppBuilder AddCloudFoundryActuators(this AppBuilder instance, string basePath = null)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore.Add("Logging:LogLevel:Default", "Information");
            inMemoryConfigStore.Add("Logging:LogLevel:System", "Warning");
            inMemoryConfigStore.Add("Logging:LogLevel:Microsoft", "Warning");
            inMemoryConfigStore.Add("Logging:LogLevel:Steeltoe", "Warning");
            inMemoryConfigStore.Add("Logging:LogLevel:Pivotal", "Warning");
            inMemoryConfigStore.Add("Logging:Console:IncludeScopes", "true");

            if (string.IsNullOrWhiteSpace(basePath))
                inMemoryConfigStore.Add("management:endpoints:path", "/cloudfoundryapplication");
            else
                inMemoryConfigStore.Add("management:endpoints:path", $"{basePath.TrimEnd('/')}/cloudfoundryapplication");

            inMemoryConfigStore.Add("management:endpoints:cloudfoundry:validateCertificates", "false");

            inMemoryConfigStore.Add("info:ApplicationName", "${vcap:application:name}");
            inMemoryConfigStore.Add("info:CurrentEnvironment", "${ASPNETCORE_ENVIRONMENT}");
            inMemoryConfigStore.Add("info:AssemblyInfo", Assembly.GetCallingAssembly().FullName);

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfActuator());

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }

        public static AppBuilder AddCloudFoundryMetricsForwarder(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore.Add("management:metrics:exporter:cloudfoundry:validateCertificates", "false");

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<IActuator>>(instance, "Actuators").Add(new CfMetricsForwarder());

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }
    }
}
