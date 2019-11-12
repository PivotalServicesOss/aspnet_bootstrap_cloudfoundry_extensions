using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests
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
        public async Task Test_ContinueNextShouldReturnTrue()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(await handler.ContinueNextAsync(null));
        }

        [Fact]
        public async Task Test_IsEnabledShouldReturnTrue()
        {
            var handler = new InboundErrorRequestObserverHandler(observer.Object, logger.Object);
            Assert.True(await handler.IsEnabledAsync(null));
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
