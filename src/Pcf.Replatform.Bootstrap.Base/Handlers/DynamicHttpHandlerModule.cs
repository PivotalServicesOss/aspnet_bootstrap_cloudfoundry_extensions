using System.Collections.Generic;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public class DynamicHttpHandlerModule : IHttpModule
    {
        internal static List<IDynamicHttpHandler> Handlers { get; } = new List<IDynamicHttpHandler>();

        public DynamicHttpHandlerModule() { }

        public void Init(HttpApplication application)
        {
            foreach (var handler in Handlers)
                handler.RegisterEvent(application);
        }

        public void Dispose()
        { }
    }
}