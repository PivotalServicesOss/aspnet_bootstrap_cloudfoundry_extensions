using Microsoft.AspNetCore.Authentication;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public interface ISpnegoAuthenticator
    {
        AuthenticateResult Authenticate(HttpContextBase contextBase);
        void Challenge(AuthenticationProperties properties, HttpContextBase contextBase);
    }
}