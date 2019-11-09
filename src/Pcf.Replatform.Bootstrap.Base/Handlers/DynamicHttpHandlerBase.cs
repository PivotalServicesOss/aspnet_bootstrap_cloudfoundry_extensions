using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public abstract class DynamicHttpHandlerBase : IDynamicHttpHandler
    {
        protected ILogger<DynamicHttpHandlerBase> logger;

        public DynamicHttpHandlerBase(ILogger<DynamicHttpHandlerBase> logger)
        {
            this.logger = logger;
        }

        public DynamicHttpHandlerBase()
            : this(DependencyContainer.GetService<ILogger<DynamicHttpHandlerBase>>(true))
        {
        }

        protected abstract string Path { get; }

        public abstract void HandleRequest(HttpContextBase context);

        public virtual async Task<bool> IsAllowedAsync(HttpContextBase context)
        {
            //Authorize here if you need to restricted access
            return await Task.FromResult(result: true);
        }

        public bool IsPathMatched(HttpContextBase context)
        {
            if (context.Request.Path.Contains(Path))
                return true;

            return false;
        }

        protected internal string GetRequestUri(HttpRequestBase request)
        {
            string str = request.IsSecureConnection ? "https" : "http";
            string text = request.Headers.Get("X-Forwarded-Proto");
            if (text != null)
                str = text;

            return str + "://" + request.Url.Host + request.Path.ToString();
        }
    }
}