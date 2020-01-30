using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xunit;

namespace PCF.Replat.Bootstrap.WinAuth.Tests.Handlers
{
    public class WindowsAuthenticationHandlerTests
    {
        Mock<ICookieAuthenticator> cookieAuthenticator;
        Mock<ISpnegoAuthenticator> spnegoAuthenticator;
        Mock<ILogger<WindowsAuthenticationHandler>> logger;
        Mock<HttpServerUtilityBase> server;
        Mock<HttpResponseBase> response;
        Mock<HttpRequestBase> request;
        Mock<HttpSessionStateBase> session;
        Mock<HttpContextBase> context;
        HttpCookieCollection cookies;

        public WindowsAuthenticationHandlerTests()
        {
            cookieAuthenticator = new Mock<ICookieAuthenticator>();
            spnegoAuthenticator = new Mock<ISpnegoAuthenticator>();
            logger = new Mock<ILogger<WindowsAuthenticationHandler>>();
            server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            session = new Mock<HttpSessionStateBase>();
            context = new Mock<HttpContextBase>();
            cookies = new HttpCookieCollection();
            SetHttpContext();
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

        [Fact]
        public void Test_UsesCookieAuthenticator_ToSet_UserPrincipal_IfCookieAuthenticatorReturnsSuccess()
        {
            var serializer = new TicketSerializer();
            var ticket = new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Foo User"),
                                }, AuthConstants.SPNEGO_DEFAULT_SCHEME)),
                            AuthConstants.SPNEGO_DEFAULT_SCHEME);
            var encodedTicket = Convert.ToBase64String(serializer.Serialize(ticket));

            var cookie = new HttpCookie(AuthConstants.AUTH_COOKIE_NM)
            {
                Expires = DateTime.Now.AddDays(90),
                Value = encodedTicket
            };

            cookies.Set(cookie);
            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.Success(ticket));

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object);
            handler.HandleRequest(context.Object);

            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Never);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny< AuthenticateResult>(), context.Object), Times.Never);
            context.VerifySet(c => c.User = ticket.Principal, Times.Once);
        }

        [Fact]
        public void Test_UsesSpnegoAuthenticator_ToSet_UserPrincipal_IfCookieAuthenticatorReturnsNotSuccess()
        {
            var serializer = new TicketSerializer();
            var ticket = new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Foo User"),
                                }, AuthConstants.SPNEGO_DEFAULT_SCHEME)),
                            AuthConstants.SPNEGO_DEFAULT_SCHEME);
            var encodedTicket = Convert.ToBase64String(serializer.Serialize(ticket));

            var cookie = new HttpCookie(AuthConstants.AUTH_COOKIE_NM)
            {
                Expires = DateTime.Now.AddDays(90),
                Value = encodedTicket
            };

            cookies.Set(cookie);
            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());
            spnegoAuthenticator.Setup(sa => sa.Authenticate(context.Object)).Returns(AuthenticateResult.Success(ticket));

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Never);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Once);
            context.VerifySet(c => c.User = ticket.Principal, Times.Once);
        }

        [Fact]
        public void Test_IfChallengeIsCalled_If_Both_Authenticators_ReturnsNotSuccess()
        {
            var serializer = new TicketSerializer();
            var ticket = new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Foo User"),
                                }, AuthConstants.SPNEGO_DEFAULT_SCHEME)),
                            AuthConstants.SPNEGO_DEFAULT_SCHEME);
            var encodedTicket = Convert.ToBase64String(serializer.Serialize(ticket));

            var cookie = new HttpCookie(AuthConstants.AUTH_COOKIE_NM)
            {
                Expires = DateTime.Now.AddDays(90),
                Value = encodedTicket
            };

            cookies.Set(cookie);
            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());
            spnegoAuthenticator.Setup(sa => sa.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Once);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Never);
            context.VerifySet(c => c.User = ticket.Principal, Times.Never);
        }

        private void SetHttpContext()
        {
            request.Setup(r => r.UserHostAddress).Returns("127.0.0.1");
            session.Setup(s => s.SessionID).Returns(Guid.NewGuid().ToString());
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Server).Returns(server.Object);
            context.SetupGet(c => c.Session).Returns(session.Object);
            request.SetupGet(r => r.Cookies).Returns(cookies);
        }
    }
}
