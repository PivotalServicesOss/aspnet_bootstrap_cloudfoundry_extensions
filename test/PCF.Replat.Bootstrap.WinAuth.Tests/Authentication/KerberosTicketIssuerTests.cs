using Kerberos.NET;
using Kerberos.NET.Crypto;
using Microsoft.Extensions.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstrap.WinAuth.Tests.Authentication
{
    public class KerberosTicketIssuerTests
    {
        public KerberosTicketIssuerTests()
        {
        }

        [Fact]
        public void Test_IsOfTypeITicketIssuer()
        {
            var configuration = GetMockConfiguration(new Dictionary<string, string>());

            Assert.IsAssignableFrom<ITicketIssuer>(new KerberosTicketIssuer(GetAuthenticatorStub("foo"), configuration));
        }

        [Fact]
        public void Test_AuthenticateThrowsException_If_PrincipalPassword_Is_Not_Set()
        {
            var configuration = GetMockConfiguration(new Dictionary<string, string>());

            var issuer = new KerberosTicketIssuer(GetAuthenticatorStub("foo"), configuration);

            Assert.Throws<ArgumentNullException>(() => issuer.Authenticate(Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World"))));
        }

        [Fact]
        public void Test_Authenticates_And_Issues_A_Ticket_If_PrincipalPassword_Is_Set()
        {
            var configuration = GetMockConfiguration(new Dictionary<string, string>() { { AuthConstants.PRINCIPAL_PASSWORD_NM, "foo" } });

            var issuer = new KerberosTicketIssuer(GetAuthenticatorStub("bar"), configuration);

            var claimsIdentity = issuer.Authenticate(Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello World")));

            Assert.Equal("bar", claimsIdentity.Principal.Identity.Name);
        }

        private IConfiguration GetMockConfiguration(Dictionary<string, string> configurations)
        {
            return new ConfigurationBuilder().AddInMemoryCollection(configurations).Build();
        }

        private KerberosAuthenticator GetAuthenticatorStub(string userName)
        {
            return new KerberosAuthenticatorStub(new KerberosValidator(new KerberosKey("pass")), userName);
        }
    }

    internal class KerberosAuthenticatorStub : KerberosAuthenticator
    {
        private readonly string userName;

        public KerberosAuthenticatorStub(IKerberosValidator validator, string userName) : base(validator)
        {
            this.userName = userName;
        }

        public override Task<ClaimsIdentity> Authenticate(string token)
        {
            return Task.FromResult(new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name, userName)
                                }));
        }
    }
}
