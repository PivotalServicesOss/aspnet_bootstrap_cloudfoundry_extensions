using Microsoft.AspNetCore.Authentication;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public interface ITicketIssuer
    {
        AuthenticationTicket Authenticate(string base64Token);
    }
}
