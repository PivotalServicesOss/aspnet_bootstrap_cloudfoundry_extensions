using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using System.Web;
using Xunit;

//Minimum tests are added
namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class InboundRequestObserverModuleTests
    {
        public InboundRequestObserverModuleTests()
        {
        }

        [Fact]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.True(new InboundRequestObserverModule() is IHttpModule);
        }
    }
}
