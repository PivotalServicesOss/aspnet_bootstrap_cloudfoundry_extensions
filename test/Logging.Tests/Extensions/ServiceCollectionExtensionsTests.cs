using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Xunit;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Testing;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Observers;

//Minimum tests are added
namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Tests.Extensions
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
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IInboundRequestObserver); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(ITracing); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IDynamicMessageProcessor); });
            Assert.Contains(services, (sd) => { return sd.ServiceType == typeof(IDiagnosticsManager); });
        }
    }
}
