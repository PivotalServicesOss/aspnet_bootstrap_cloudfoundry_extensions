using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.WinAuth.Tests.Handlers
{
    public class WindowsAuthenticationHandlerTests
    {
        Mock<ICookieAuthenticator> cookieAuthenticator;
        Mock<ISpnegoAuthenticator> spnegoAuthenticator;
        Mock<ILogger<WindowsAuthenticationHandler>> logger;

        public WindowsAuthenticationHandlerTests()
        {
            cookieAuthenticator = new Mock<ICookieAuthenticator>();
            spnegoAuthenticator = new Mock<ISpnegoAuthenticator>();
            logger = new Mock<ILogger<WindowsAuthenticationHandler>>();
        }

        [Fact]
        public void Test_IsOfType_DynamicHttpHandlerBase()
        {
            Assert.IsAssignableFrom<DynamicHttpHandlerBase>(new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEvent_Is_AuthenticateRequest()
        {
            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.AuthenticateRequest, handler.ApplicationEvent);
        }
    }
}
