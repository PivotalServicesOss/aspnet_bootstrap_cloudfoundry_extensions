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

        public abstract string Path { get; }

        public abstract void HandleRequest(HttpContextBase context);

        /// <summary>
        /// Registers AddOnPostAuthorizeRequestAsync by default
        /// </summary>
        /// <param name="context"></param>
        /// <param name="eventHandlerHelper"></param>
        public virtual void RegisterEvent(HttpApplication application, EventHandlerTaskAsyncHelper eventHandlerHelper)
        {
            application.AddOnPostAuthorizeRequestAsync(eventHandlerHelper.BeginEventHandler, eventHandlerHelper.EndEventHandler);
        }

        /// <summary>
        /// Default is true, but access can be restricted based on permission here
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsEnabledAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: true);
        }

        /// <summary>
        /// Should continue processing the request after this handler, default is false
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task<bool> ContinueNextAsync(HttpContextBase context)
        {
            return await Task.FromResult(result: false);
        }

        internal bool IsPathMatched(HttpContextBase context)
        {
            if (!string.IsNullOrWhiteSpace(Path) && context.Request.Path.Contains(Path))
                return true;
            else if (string.IsNullOrWhiteSpace(Path))
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