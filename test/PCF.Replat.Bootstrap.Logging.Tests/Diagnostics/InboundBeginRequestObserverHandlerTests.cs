using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests
{
    public class InboundBeginRequestObserverHandlerTests
    {
        Mock<IInboundRequestObserver> observer;
        Mock<ILogger<InboundBeginRequestObserverHandler>> logger;

        public InboundBeginRequestObserverHandlerTests()
        {
            observer = new Mock<IInboundRequestObserver>();
            logger = new Mock<ILogger<InboundBeginRequestObserverHandler>>();
        }

        [Fact]
        public void Test_IsTypeOfHttpHandler()
        {
            Assert.True(new InboundBeginRequestObserverHandler(observer.Object, logger.Object) is DynamicHttpHandlerBase);
        }

        [Fact]
        public void Test_ContinueNextShouldReturnTrue()
        {
            var handler = new InboundBeginRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(handler.ContinueNext(null));
        }

        [Fact]
        public void Test_IsEnabledShouldReturnTrue()
        {
            var handler = new InboundBeginRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(handler.IsEnabled(null));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new InboundBeginRequestObserverHandler(observer.Object, logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEventIsError()
        {
            var handler = new InboundBeginRequestObserverHandler(observer.Object, logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.BeginRequest, handler.ApplicationEvent);
        }
    }
}
