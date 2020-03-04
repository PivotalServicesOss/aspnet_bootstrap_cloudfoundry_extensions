using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Tests
{
    public class InboundErrorRequestObserverHandlerTests
    {
        Mock<IInboundRequestObserver> observer;
        Mock<ILogger<InboundErrorRequestObserverHandler>> logger;

        public InboundErrorRequestObserverHandlerTests()
        {
            observer = new Mock<IInboundRequestObserver>();
            logger = new Mock<ILogger<InboundErrorRequestObserverHandler>>();
        }

        [Fact]
        public void Test_IsTypeOfHttpHandler()
        {
            Assert.True(new InboundErrorRequestObserverHandler(observer.Object, logger.Object) is DynamicHttpHandlerBase);
        }

        [Fact]
        public void Test_ContinueNextShouldReturnTrue()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(handler.ContinueNext(null));
        }

        [Fact]
        public void Test_IsEnabledShouldReturnTrue()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(handler.IsEnabled(null));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEventIsError()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.Error, handler.ApplicationEvent);
        }
    }
}
