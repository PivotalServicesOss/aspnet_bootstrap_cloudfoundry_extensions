using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
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
            response = new Mock<HttpResponseBase>(MockBehavior.Loose);
            request = new Mock<HttpRequestBase>(MockBehavior.Loose);
            session = new Mock<HttpSessionStateBase>();
            context = new Mock<HttpContextBase>();
            cookies = new HttpCookieCollection();
            SetHttpContext();
        }

        [Fact]
        public void Test_IsOfType_DynamicHttpHandlerBase()
        {
            Assert.IsAssignableFrom<DynamicHttpHandlerBase>(new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object));
        }

        [Fact]
        public void Test_PathIsNull()
        {
            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
            Assert.Null(handler.Path);
        }

        [Fact]
        public void Test_ApplicationEvent_Is_PostAuthenticateRequest()
        {
            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
            Assert.Equal(DynamicHttpHandlerEvent.PostAuthenticateRequest, handler.ApplicationEvent);
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
                Expires = DateTime.Now.AddDays(1),
                Value = encodedTicket
            };

            cookies.Set(cookie);
            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.Success(ticket));

            var whitelistPath = "/whitelistpath";
            var mockConfiguration = new Dictionary<string, string>() { { AuthConstants.WHITELIST_PATHS_CSV_NM, whitelistPath } };
            request.SetupGet(r => r.Path).Returns("/anypath");

            context.SetupGet(c => c.User).Returns(()=> { return null; });

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
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
                Expires = DateTime.Now.AddDays(1),
                Value = encodedTicket
            };

            cookies.Set(cookie);
            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());
            spnegoAuthenticator.Setup(sa => sa.Authenticate(context.Object)).Returns(AuthenticateResult.Success(ticket));

            var whitelistPath = "/whitelistpath";
            var mockConfiguration = new Dictionary<string, string>() { { AuthConstants.WHITELIST_PATHS_CSV_NM, whitelistPath } };
            request.SetupGet(r => r.Path).Returns("/anypath");

            context.SetupGet(c => c.User).Returns(() => { return null; });

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
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
                Expires = DateTime.Now.AddDays(1),
                Value = encodedTicket
            };

            cookies.Set(cookie);

            cookieAuthenticator.Setup(ca => ca.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());
            spnegoAuthenticator.Setup(sa => sa.Authenticate(context.Object)).Returns(AuthenticateResult.NoResult());

            var whitelistPath = "/whitelistpath";
            var mockConfiguration = new Dictionary<string, string>() { { AuthConstants.WHITELIST_PATHS_CSV_NM, whitelistPath } };
            request.SetupGet(r => r.Path).Returns("/anypath");

            context.SetupGet(c => c.User).Returns(() => { return null; });

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Once);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Once);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Never);
            context.VerifySet(c => c.User = ticket.Principal, Times.Never);
        }

        [Fact]
        public void Test_If_NoneOfThe_Authenticators_Are_Called_If_Whitelisted()
        {
            var whitelistPath = "/whitelistpath";
            var mockConfiguration = new Dictionary<string, string>() { { AuthConstants.WHITELIST_PATHS_CSV_NM, whitelistPath } };
            request.SetupGet(r => r.Path).Returns("/whitelistpath");

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(mockConfiguration), logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Never);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Never);
        }

        //For cloud foundry actuators
        [InlineData("/cloudfoundryapplication")]
        [InlineData("/cloudfoundryapplication/")]
        [InlineData("/actuator")]
        [InlineData("/actuator/")]
        [Theory]
        public void Test_If_NoneOfThe_Authenticators_Are_Called_ForDefaultWhitelistedPath(string path)
        {
            request.SetupGet(r => r.Path).Returns(path);

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(), logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Never);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Never);
        }


        [Fact]
        public void Test_If_NoneOfThe_Authenticators_Are_Called_If_Already_Whitelisted_And_Authenticated()
        {
            var claimsPrincipal = new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Foo User"),
                                }, AuthConstants.SPNEGO_DEFAULT_SCHEME));

            var whitelistPath = "/whitelistpath";
            var mockConfiguration = new Dictionary<string, string>() { { AuthConstants.WHITELIST_PATHS_CSV_NM, whitelistPath } };
            request.SetupGet(r => r.Path).Returns("/whitelistpath");

            context.SetupGet(c => c.User).Returns(claimsPrincipal);

            var handler = new WindowsAuthenticationHandler(cookieAuthenticator.Object, spnegoAuthenticator.Object, GetConfiguration(mockConfiguration), logger.Object);
            handler.HandleRequest(context.Object);

            cookieAuthenticator.Verify(ca => ca.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Authenticate(context.Object), Times.Never);
            spnegoAuthenticator.Verify(sa => sa.Challenge(It.IsAny<AuthenticationProperties>(), context.Object), Times.Never);
            cookieAuthenticator.Verify(ca => ca.SignIn(It.IsAny<AuthenticateResult>(), context.Object), Times.Never);
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

        private IConfiguration GetConfiguration(Dictionary<string, string> keyValuePairs = null)
        {
            return new ConfigurationBuilder().AddInMemoryCollection(keyValuePairs ?? new Dictionary<string, string>()).Build();
        }
    }
}
