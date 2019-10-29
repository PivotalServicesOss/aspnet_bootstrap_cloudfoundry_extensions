using Pivotal.CloudFoundry.Replatform.Bootstrap.Base;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Actuators
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder AddHealthActuators(this AppBuilder instance, string basePath = null)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                instance.InMemoryConfigStore.Add("management:endpoints:path", "/cloudfoundryapplication");
            else
                instance.InMemoryConfigStore.Add("management:endpoints:path", $"{basePath.TrimEnd('/')}/cloudfoundryapplication");

            instance.InMemoryConfigStore.Add("management:endpoints:cloudfoundry:validateCertificates", "false");

            instance.Actuators.Add(new CfActuator());

            instance.ConfigureServicesDelegates.Add((builderContext, services) => {
                services.AddControllers();
            });
            return instance;

            return instance;
        }

        public static AppBuilder AddMetricsForwarder(this AppBuilder instance)
        {
            instance.InMemoryConfigStore.Add("management:metrics:exporter:cloudfoundry:validateCertificates", "false");

            instance.Actuators.Add(new CfMetricsForwarder());
            return instance;
        }
    }
}
