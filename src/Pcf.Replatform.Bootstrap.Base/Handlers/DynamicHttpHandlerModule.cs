using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System.Collections.Generic;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public class DynamicHttpHandlerModule : IHttpModule
    {
        internal static List<IDynamicHttpHandler> Handlers { get; } = new List<IDynamicHttpHandler>();

        private ILogger<DynamicHttpHandlerModule> logger;

        public DynamicHttpHandlerModule() { }

        public void Init(HttpApplication application)
        {
            logger = DependencyContainer.GetService<ILogger<DynamicHttpHandlerModule>>(true);

            foreach (var handler in Handlers)
                handler.RegisterEvent(application);
        }

        public void Dispose()
        { }
    }
}