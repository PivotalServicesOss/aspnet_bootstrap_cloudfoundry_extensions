using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    //[Obsolete("Only meant for testing purpose")]
    //internal class TestTicketIssuer : ITicketIssuer
    //{
    //    public AuthenticationTicket Authenticate(string base64Token)
    //    {
    //        return new AuthenticationTicket(
    //                        new ClaimsPrincipal(
    //                            new ClaimsIdentity(new[]
    //                            {
    //                                    new Claim(ClaimTypes.Name,"Test User"),
    //                            }, AuthConstants.SPNEGO_DEFAULT_SCHEME)),
    //                        AuthConstants.SPNEGO_DEFAULT_SCHEME);
    //    }
    //}
}
