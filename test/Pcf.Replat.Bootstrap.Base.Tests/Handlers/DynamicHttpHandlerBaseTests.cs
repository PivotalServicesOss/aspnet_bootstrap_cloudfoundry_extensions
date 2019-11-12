using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using System;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace PCF.Replat.Bootstrap.Base.Tests.Handlers
{
    public class DynamicHttpHandlerBaseTests
    {
        Mock<ILogger<DynamicHttpHandlerSpy>> logger;
        public DynamicHttpHandlerBaseTests()
        {
            logger = new Mock<ILogger<DynamicHttpHandlerSpy>>();
        }

        [Fact]
        public async Task Test_ContinueNextShouldReturnFalseByDefault()
        {
            var handler = new DynamicHttpHandlerSpy(logger.Object);
            Assert.False(await handler.ContinueNextAsync(null));
        }

        [Fact]
        public async Task Test_IsEnabledShouldReturnTrueByDefault()
        {
            var handler = new DynamicHttpHandlerSpy(logger.Object);
            Assert.True(await handler.IsEnabledAsync(null));
        }
    }

    public class DynamicHttpHandlerSpy : DynamicHttpHandlerBase
    {
        public DynamicHttpHandlerSpy(ILogger<DynamicHttpHandlerSpy> logger) : base(logger)
        {
        }

        public override string Path => throw new NotImplementedException();

        public override DynamicHttpHandlerEvent ApplicationEvent => throw new NotImplementedException();

        public override void HandleRequest(HttpContextBase context)
        {
            throw new NotImplementedException();
        }
    }
}
