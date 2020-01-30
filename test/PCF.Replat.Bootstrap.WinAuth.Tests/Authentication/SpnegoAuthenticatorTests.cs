using Kerberos.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Xunit;

namespace PCF.Replat.Bootstrap.WinAuth.Tests.Authentication
{
    public class SpnegoAuthenticatorTests
    {
        Mock<ITicketIssuer> issuer;
        Mock<ILogger<SpnegoAuthenticator>> logger;
        Mock<HttpServerUtilityBase> server;
        Mock<HttpResponseBase> response;
        Mock<HttpRequestBase> request;
        Mock<HttpSessionStateBase> session;
        Mock<HttpContextBase> context;
        NameValueCollection headers;

        public SpnegoAuthenticatorTests()
        {
            issuer = new Mock<ITicketIssuer>();
            logger = new Mock<ILogger<SpnegoAuthenticator>>();
            server = new Mock<HttpServerUtilityBase>(MockBehavior.Loose);
            response = new Mock<HttpResponseBase>();
            request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            session = new Mock<HttpSessionStateBase>();
            context = new Mock<HttpContextBase>();
            headers = new NameValueCollection();
            SetHttpContext();
        }

        [Fact]
        public void Test_IsOfTypeISpnegoAuthenticator()
        {
            Assert.IsAssignableFrom<ISpnegoAuthenticator>(new SpnegoAuthenticator(issuer.Object, logger.Object));
        }

        [Fact]
        public void Test_ThrowsArgumentNullExceptionIfHttpContextIsNull()
        {
            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            Assert.Throws<ArgumentNullException>(() => authenticator.Authenticate(null));
            issuer.Verify(i => i.Authenticate(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test_Returns_NotSuccess_If_Authorization_HeaderIsNull()
        {
            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            issuer.Verify(i => i.Authenticate(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test_Returns_NotSuccess_If_Authorization_HeaderIsEmpty()
        {
            headers.Add("Authorization", string.Empty);

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            issuer.Verify(i => i.Authenticate(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test_Returns_NotSuccess_If_Authorization_Header_DoesNotStartsWithNegotiate()
        {
            headers.Add("Authorization", "foo");

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            issuer.Verify(i => i.Authenticate(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test_Returns_NotSuccess_NoCredentialsMessage_If_Authorization_Header_StartsWithNegotiate_ButTokenIsEmpty()
        {
            headers.Add("Authorization", $"{AuthConstants.SPNEGO_DEFAULT_SCHEME} ");

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            Assert.Equal("No credentials", result.Failure.Message);
            issuer.Verify(i => i.Authenticate(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Test_Returns_Success_If_IssuerAuthenticatesWithAValidToken()
        {
            headers.Add("Authorization", $"{AuthConstants.SPNEGO_DEFAULT_SCHEME} SOMEBASE64TOKEN");

            var ticket = new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Foo User"),
                                }, AuthConstants.SPNEGO_DEFAULT_SCHEME)),
                            AuthConstants.SPNEGO_DEFAULT_SCHEME);

            issuer.Setup(i => i.Authenticate("SOMEBASE64TOKEN")).Returns(ticket);

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.True(result.Succeeded);
            Assert.Equal("Foo User", result.Principal.Identity.Name);
            issuer.Verify(i => i.Authenticate(It.Is<string>(s => s == "SOMEBASE64TOKEN")), Times.Once);
        }

        [Fact]
        public void Test_Returns_FailureWithExceptionMessage_If_IssuerThrowsKerberosValidationException()
        {
            headers.Add("Authorization", "Negotiate SOMEBASE64TOKEN");

            issuer.Setup(i => i.Authenticate("SOMEBASE64TOKEN")).Returns(() => throw new KerberosValidationException("bar"));

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            Assert.Equal("bar", result.Failure.Message);
            issuer.Verify(i => i.Authenticate(It.Is<string>(s => s == "SOMEBASE64TOKEN")), Times.Once);
        }

        [Fact]
        public void Test_Returns_FailureWithAccessDeniedMessage_If_IssuerThrowsAnyException()
        {
            headers.Add("Authorization", "Negotiate SOMEBASE64TOKEN");

            issuer.Setup(i => i.Authenticate("SOMEBASE64TOKEN")).Returns(() => throw new Exception());

            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            var result = authenticator.Authenticate(context.Object);

            Assert.False(result.Succeeded);
            Assert.Equal("Access denied!", result.Failure.Message);
            issuer.Verify(i => i.Authenticate(It.Is<string>(s => s == "SOMEBASE64TOKEN")), Times.Once);
        }

        [Fact]
        public void Test_Challenge_SetsStatus401_And_NecessaryHeaders_InTheResponse()
        {
            var authenticator = new SpnegoAuthenticator(issuer.Object, logger.Object);
            authenticator.Challenge(null, context.Object);

            response.VerifySet(r => r.StatusCode = 401);
            Assert.NotNull(response.Object.Headers[HeaderNames.WWWAuthenticate]);
            Assert.Equal(AuthConstants.SPNEGO_DEFAULT_SCHEME, response.Object.Headers[HeaderNames.WWWAuthenticate]);
        }

        private void SetHttpContext()
        {
            request.Setup(r => r.UserHostAddress).Returns("127.0.0.1");
            session.Setup(s => s.SessionID).Returns(Guid.NewGuid().ToString());
            context.SetupGet(c => c.Request).Returns(request.Object);
            context.SetupGet(c => c.Response).Returns(response.Object);
            context.SetupGet(c => c.Server).Returns(server.Object);
            context.SetupGet(c => c.Session).Returns(session.Object);
            request.SetupGet(r => r.Url).Returns(new Uri("http://localhost"));
            request.SetupGet(r => r.Headers).Returns(headers);
            response.SetupGet(r => r.Headers).Returns(headers);
        }
    }
}
