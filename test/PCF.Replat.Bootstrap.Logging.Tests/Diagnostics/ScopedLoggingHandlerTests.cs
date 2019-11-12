using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests
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
        public async Task Test_ContinueNextShouldReturnTrue()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.True(await handler.ContinueNextAsync(null));
        }

        [Fact]
        public async Task Test_IsEnabledShouldReturnTrue()
        {
            var handler = new ScopedLoggingHandler(logger.Object);
            Assert.True(await handler.IsEnabledAsync(null));
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
