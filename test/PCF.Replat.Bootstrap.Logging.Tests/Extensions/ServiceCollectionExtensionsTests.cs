using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using Xunit;
using PCF.Replatform.Test.Helpers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Testing;

//Minimum tests are added
namespace PCF.Replat.Bootstrap.Logging.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Test_AddDefaultDiagnosticsDependencies_InjectsNecessaryDependencies()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            TestProxy.AddDefaultDiagnosticsDependencies(services, configuration);

            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(ITracingOptions); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IDiagnosticObserver); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(ITracing); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IDynamicMessageProcessor); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IDiagnosticsManager); });
        }
    }
}
