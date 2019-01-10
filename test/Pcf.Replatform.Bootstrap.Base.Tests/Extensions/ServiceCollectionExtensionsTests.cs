using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using System.Linq;

//Minimum tests are added
namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Extensions
{
    [TestClass]
    public class ServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void Test_AddDefaultDiagnosticsDependencies_InjectsNecessaryDependencies()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build();

            services.AddDefaultDiagnosticsDependencies(configuration);

            Assert.IsTrue(services.Any((sd) => { return sd.ServiceType == typeof(ITracingOptions); }));
            Assert.IsTrue(services.Any((sd) => { return sd.ServiceType == typeof(IDiagnosticObserver); }));
            Assert.IsTrue(services.Any((sd) => { return sd.ServiceType == typeof(ITracing); }));
            Assert.IsTrue(services.Any((sd) => { return sd.ServiceType == typeof(IDynamicMessageProcessor); }));
            Assert.IsTrue(services.Any((sd) => { return sd.ServiceType == typeof(IDiagnosticsManager); }));
        }
    }
}
