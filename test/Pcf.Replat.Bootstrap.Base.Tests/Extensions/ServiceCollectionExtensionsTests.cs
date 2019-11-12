using Microsoft.Extensions.DependencyInjection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using System.Linq;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace Pcf.Replat.Bootstrap.Base.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void Test_AddControllers_AddsOnlyClassesEndsWithControllersAndImplementationOfIController()
        {
            var services = new ServiceCollection();

            TestProxy.AddControllersProxy(services);

            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "Pcf.Replat.Bootstrap.Base.Tests.TestApiController");
            Assert.Contains(services, (desc) => desc?.ImplementationType?.FullName == "Pcf.Replat.Bootstrap.Base.Tests.TestMvcController");
            Assert.True(!services.Any((desc) => desc?.ImplementationType?.FullName == "Pcf.Replat.Bootstrap.Base.Tests.TestController3"));
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete



