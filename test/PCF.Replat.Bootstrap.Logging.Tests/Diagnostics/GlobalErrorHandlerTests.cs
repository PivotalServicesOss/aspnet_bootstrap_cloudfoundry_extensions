using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class GlobalErrorHandlerTests
    {
        Mock<ILogger<GlobalErrorHandler>> logger;

        public GlobalErrorHandlerTests()
        {
            logger = new Mock<ILogger<GlobalErrorHandler>>();
        }

        [Fact]
        public void Test_IsTypeOfHttpHandler()
        {
            Assert.True(new GlobalErrorHandler(logger.Object) is DynamicHttpHandlerBase);
        }

        [Fact]
        public void Test_ContinueNextShouldReturnTrue()
        {
            var handler = new GlobalErrorHandler(logger.Object);
            Assert.True(handler.ContinueNext(null));
        }

        [Fact]
        public void Test_IsEnabledShouldReturnTrue()
        {
            var handler = new GlobalErrorHandler(logger.Object);
            Assert.True(handler.IsEnabled(null));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new GlobalErrorHandler(logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEventIsError()
        {
            var handler = new GlobalErrorHandler(logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.Error, handler.ApplicationEvent);
        }
    }
}
