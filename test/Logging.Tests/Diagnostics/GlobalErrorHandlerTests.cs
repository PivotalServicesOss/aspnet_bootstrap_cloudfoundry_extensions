using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers;
using Xunit;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Tests
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
