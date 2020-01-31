using Kerberos.NET;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using System;
using System.Security.Claims;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public class KerberosTicketIssuer : ITicketIssuer
    {
        private readonly IConfiguration configuration;
        private readonly KerberosAuthenticator authenticator;

        public KerberosTicketIssuer(KerberosAuthenticator authenticator, IConfiguration configuration)
        {
            this.authenticator = authenticator ?? throw new ArgumentNullException(nameof(authenticator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public AuthenticationTicket Authenticate(string base64Token)
        {
            if (string.IsNullOrWhiteSpace(configuration[AuthConstants.PRINCIPAL_PASSWORD_NM])
                    || configuration[AuthConstants.PRINCIPAL_PASSWORD_NM] == AuthConstants.PRINCIPAL_PASSWORD_FROM_CREDHUB)
            {
                throw new ArgumentNullException($"{AuthConstants.PRINCIPAL_PASSWORD_NM} is not set! 1. Set as an environment variable, 2. Set it in credhub with key `principal_password` so that it will be pulled automatically using placeholder resolver `{AuthConstants.PRINCIPAL_PASSWORD_FROM_CREDHUB}`");
            }

            var identity = authenticator.Authenticate(base64Token).Result;

            return new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                AuthConstants.SPNEGO_DEFAULT_SCHEME);
        }
    }
}
