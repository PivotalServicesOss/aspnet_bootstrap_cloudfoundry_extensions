using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public interface IDynamicHttpHandler
    {
        void HandleRequest(HttpContextBase context);
        Task<bool> IsAllowedAsync(HttpContextBase context);
        bool IsPathMatched(HttpContextBase context);
    }
}