using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using System.Web;
using Xunit;

//Minimum tests are added
namespace PCF.Replat.Bootstrap.Logging.Tests
{
     public class ScopedLoggingModuleTests
    {
        [Fact]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.True(new ScopedLoggingModule() is IHttpModule);
        }
    }
}
