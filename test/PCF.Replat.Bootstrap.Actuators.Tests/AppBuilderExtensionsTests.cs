using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using Xunit;

namespace PCF.Replat.Bootstrap.Actuators.Tests
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_AddHealthActuators_AddsIntoActuatorCollection()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddHealthActuators();
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
            Assert.Equal("/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
        }

        [Fact]
        public void Test_AddHealthActuators_AddsIntoActuatorCollection_WithBasePath()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddHealthActuators("/foo");
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
            Assert.Equal("/foo/cloudfoundryapplication", TestProxy.InMemoryConfigStoreProxy["management:endpoints:path"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:endpoints:cloudfoundry:validateCertificates"]);
        }

        [Fact]
        public void Test_AddMetricsForwarder_AddsIntoActuatorCollection()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ActuatorsProxy.Clear();
            AppBuilder.Instance.AddMetricsForwarder();
            Assert.Single(TestProxy.ActuatorsProxy);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:metrics:exporter:cloudfoundry:validateCertificates"]);
        }
    }
}
