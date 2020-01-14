using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers
{
    public interface IAuthenticator
    {
        Task<AuthenticateResult> AuthenticateAsync(HttpContextBase contextBase);
        Task ChallengeAsync(AuthenticationProperties properties, HttpContextBase contextBase);
    }
}