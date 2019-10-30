using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using System.Web;
using Xunit;

//Minimum tests are added
namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class RequestLoggerModuleTests
    {
        [Fact]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.True(new RequestLoggerModule() is IHttpModule);
        }
    }
}
