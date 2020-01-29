using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    internal class TestTicketIssuer : ITicketIssuer
    {
        public AuthenticationTicket Authenticate(string base64Token)
        {
            return new AuthenticationTicket(
                            new ClaimsPrincipal(
                                new ClaimsIdentity(new[]
                                {
                                        new Claim(ClaimTypes.Name,"Alfus"),
                                }, "Negotiate")),
                            "Negotiate");
        }
    }
}
