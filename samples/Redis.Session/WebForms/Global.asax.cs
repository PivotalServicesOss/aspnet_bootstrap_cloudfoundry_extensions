using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Mvc;
using PivotalServices.AspNet.Bootstrap.Extensions;

namespace WebForms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppBuilder.Instance 
                    .PersistSessionToRedis()
                    .Build()
                    .Start();
        }

        void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            Console.Error.WriteLine(ex);
        }
    }
}