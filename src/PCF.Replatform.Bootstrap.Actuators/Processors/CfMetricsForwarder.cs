using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using Steeltoe.Management.Census.Stats;
using Steeltoe.Management.Exporter.Metrics;
using Steeltoe.Management.Exporter.Metrics.CloudFoundryForwarder;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
{
    public class CfMetricsForwarder : IActuator
    {
        private IMetricsExporter metricsExporter;

        public void Configure()
        {
            var configuration = DependencyContainer.GetService<IConfiguration>();
            var loggerFactory = DependencyContainer.GetService<ILoggerFactory>();

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
