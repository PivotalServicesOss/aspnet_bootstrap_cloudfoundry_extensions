using DynamicHandler.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace DynamicHandler
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppBuilder.Instance
                .AddDynamicHttpHandler<AppInfoApiHandler>()
                .AddDynamicHttpHandler<SimpleAuthHandler>()
                .Build().Start();
        }
    }
}