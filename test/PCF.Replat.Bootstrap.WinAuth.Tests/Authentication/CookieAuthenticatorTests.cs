using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PCF.Replatform.Test.Helpers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Xunit;

namespace PCF.Replat.Bootstrap.WinAuth.Tests.Authentication
{
    public class CookieAuthenticatorTests
    {
        Mock<ILogger<CookieAuthenticator>> logger;
        Mock<HttpServerUtilityBase> server;
        Mock<HttpResponseBase> response;
        Mock<HttpRequestBase> request;
        Mock<HttpSessionStateBase> session;
        Mock<HttpContextBase> context;
        Mock<HttpBrowserCapabilitiesBase> browser;
        HttpCookieCollection cookies;
        Cache cache;

        public CookieAuthenticatorTests()
        {
            logger = new Mock<ILogger<CookieAuthenticator>>();
            server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            session = new Mock<HttpSessionStateBase>();
            context = new Mock<HttpContextBase>();
            browser = new Mock<HttpBrowserCapabilitiesBase>();
            cookies = new HttpCookieCollection();
            cache = new Cache();
            SetHttpContext();
        }

        [Fact]
        public void Test_IsOfTypeICookieAuthenticator()
        {
            Assert.IsAssignableFrom<ICookieAuthenticator>(new CookieAuthenticator(logger.Object));
        }

        [Fact]
        public void Test_ReturnsNoResultIfBrowserDoesNotSupportCookies()
        {
            browser.SetupGet(b => b.Cookies).Returns(false);

            var authenticator = new CookieAuthenticator(logger.Object);

            Assert.False(authenticator.Authenticate(context.Object).Succeeded);
        }

        [Fact]
        public void Test_ReturnsSuccessIfValidCookieEsists()
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

            browser.SetupGet(b => b.Cookies).Returns(true);

            var authenticator = new CookieAuthenticator(logger.Object);

            var cookieHashValue = TestHelper.InvokeNonPublicInstanceMethod(authenticator, "ComputeHash", encodedTicket);

            cache["Foo User"] = cookieHashValue;

            var result = authenticator.Authenticate(context.Object);

            Assert.True(result.Succeeded);
            Assert.Equal("Foo User", result.Principal.Identity.Name);

        }

        [Fact]
        public void Test_ReturnsFailureIf_InValidCookieEsistsOrIfCookieIsCorrupted()
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
                Value = encodedTicket + "Corrupt"
            };

            cookies.Set(cookie);

            browser.SetupGet(b => b.Cookies).Returns(true);

            var authenticator = new CookieAuthenticator(logger.Object);

            var cookieHashValue = TestHelper.InvokeNonPublicInstanceMethod(authenticator, "ComputeHash", encodedTicket);

            cache["Foo User"] = cookieHashValue;

            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            Assert.Equal($"{AuthConstants.AUTH_COOKIE_NM} cookie is corrupted!", result.Failure.Message);
        }

        [Fact]
        public void Test_ReturnsNoResultOrNotSuccessIf_CookieDoesNotEsist()
        {
            browser.SetupGet(b => b.Cookies).Returns(true);

            var authenticator = new CookieAuthenticator(logger.Object);

            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
        }

        private void SetHttpContext()
        {
            request.Setup(r => r.UserHostAddress).Returns("127.0.0.1");
            session.Setup(s => s.SessionID).Returns(Guid.NewGuid().ToString());
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Server).Returns(server.Object);
            context.SetupGet(c => c.Session).Returns(session.Object);
            context.SetupGet(c => c.Cache).Returns(cache);
            request.SetupGet(r => r.Cookies).Returns(cookies);
            request.SetupGet(r => r.Browser).Returns(browser.Object);
            request.SetupGet(r => r.Url).Returns(new Uri("http://localhost"));
        }
    }
}
