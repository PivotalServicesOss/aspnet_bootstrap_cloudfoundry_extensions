using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base;
using Steeltoe.Management.Endpoint.Metrics;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Actuators
{
    public class CfMetricsForwarder : IActuator
    {
        private IMetricsExporter metricsExporter;

        public void Configure()
        {
            var configuration = AppConfig.GetService<IConfiguration>();
            var loggerFactory = AppConfig.GetService<ILoggerFactory>();

            metricsExporter = new CloudFoundryForwarderExporter(new CloudFoundryForwarderOptions(configuration),
                                                                OpenCensusStats.Instance,
                                                                loggerFactory.CreateLogger<CloudFoundryForwarderExporter>());
        }

        public void Start()
        {
            metricsExporter.Start();
        }

        public void Stop()
        {
            metricsExporter.Stop();
        }
    }
}
