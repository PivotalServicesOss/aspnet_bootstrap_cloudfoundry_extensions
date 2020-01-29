using Microsoft.AspNetCore.Authentication;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication
{
    public interface ICookieAuthenticator
    {
        AuthenticateResult Authenticate(HttpContextBase contextBase);
        void SignIn(AuthenticateResult authResult, HttpContextBase contextBase);
    }
}