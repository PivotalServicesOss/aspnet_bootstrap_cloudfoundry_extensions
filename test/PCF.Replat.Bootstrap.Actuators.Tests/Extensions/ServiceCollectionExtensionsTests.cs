using Microsoft.Extensions.DependencyInjection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.Testing;
using System.Linq;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PCF.Replat.Bootstrap.Actuators.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Test_AddControllers_AddsOnlyClassesEndsWithControllersAndImplementationOfIController()
        {
            var services = new ServiceCollection();

            TestProxy.AddControllersProxy(services);

            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "PCF.Replat.Bootstrap.Actuators.Tests.Extensions.TestApiController");
            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "PCF.Replat.Bootstrap.Actuators.Tests.Extensions.TestMvcController");
            Assert.True(!services.Any((desc) => desc?.ImplementationType?.FullName == "PCF.Replat.Bootstrap.Actuators.Tests.Extensions.TestController3"));
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete



