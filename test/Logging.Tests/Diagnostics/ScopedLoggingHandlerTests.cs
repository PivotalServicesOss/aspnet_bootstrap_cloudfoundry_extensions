using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers;
using Xunit;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Tests
{
    public class ScopedLoggingHandlerTests
    {
        Mock<ILogger<ScopedLoggingHandler>> logger;

        public ScopedLoggingHandlerTests()
        {
            logger = new Mock<ILogger<ScopedLoggingHandler>>();
        }

        [Fact]
        public void Test_IsTypeOfHttpHandler()
        {
            Assert.True(new ScopedLoggingHandler(logger.Object) is DynamicHttpHandlerBase);
        }

        [Fact]
        public void Test_ContinueNextShouldReturnTrue()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.True(handler.ContinueNext(null));
        }

        [Fact]
        public void Test_IsEnabledShouldReturnTrue()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.True(handler.IsEnabled(null));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEventIsError()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.BeginRequest, handler.ApplicationEvent);
        }
    }
}
