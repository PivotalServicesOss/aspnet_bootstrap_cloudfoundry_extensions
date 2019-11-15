using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;

namespace WebForm
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppBuilder.Instance
                    .AddConsoleSerilogLogging(includeDistributedTracing: true)
                    .Build()
                    .Start();
        }
    }
}