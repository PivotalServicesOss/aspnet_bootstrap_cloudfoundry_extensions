using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using System.Linq;
using Xunit;

//Minimum tests are added
namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class HttpModuleConfigTests
    {
        [Fact]
        public void Test_If_GetModuleTypes_Returns_3_Modules()
        {
            Assert.Equal(3, HttpModuleConfig.GetModuleTypes().Count());
        }

        [Fact]
        public void Test_If_GetModuleTypes_Returns_InboundRequestObserverModule()
        {
            Assert.Contains(HttpModuleConfig.GetModuleTypes(), (m) => { return m == typeof(InboundRequestObserverModule); });
        }

        [Fact]
        public void Test_If_GetModuleTypes_Returns_ScopedLoggingModule()
        {
            Assert.Contains(HttpModuleConfig.GetModuleTypes(), (m) => { return m == typeof(ScopedLoggingModule); });
        }

        [Fact]
        public void Test_If_GetModuleTypes_Returns_GlobalErrorHandlerModule()
        {
            Assert.Contains(HttpModuleConfig.GetModuleTypes(), (m) => { return m == typeof(GlobalErrorHandlerModule); });
        }
    }
}
