using Kerberos.NET;
using Kerberos.NET.Crypto;
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
        private KerberosAuthenticator authenticator;

        public KerberosTicketIssuer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public AuthenticationTicket Authenticate(string base64Token)
        {
            if(authenticator == null)
            {
                if (string.IsNullOrWhiteSpace(configuration["PRINCIPAL_PASSWORD"]) 
                    || configuration["PRINCIPAL_PASSWORD"] == AppBuilderExtensions.PRINCIPAL_PASSWORD_FROM_CREDHUB)
                    throw new ArgumentNullException($"PRINCIPAL_PASSWORD is not set! 1. Set as an environment variable, 2. Set it in credhub with key `principal_password` so that it will be pulled automatically using placeholder resolver `{AppBuilderExtensions.PRINCIPAL_PASSWORD_FROM_CREDHUB}`");

                authenticator = new KerberosAuthenticator(new KerberosValidator(new KerberosKey(configuration["PRINCIPAL_PASSWORD"])))
                {
                    UserNameFormat = UserNameFormat.DownLevelLogonName
                };
            }

            var identity = authenticator.Authenticate(base64Token).Result;

            return new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                "Negotiate");
        }
    }
}
