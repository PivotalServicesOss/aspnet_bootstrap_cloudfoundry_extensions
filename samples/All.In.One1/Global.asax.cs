using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.WebHost;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using All.In.One.Handlers;
using Microsoft.Extensions.DependencyInjection;
using PivotalServices.AspNet.Bootstrap.Extensions;
using Steeltoe.Common.HealthChecks;
using All.In.One.Health;
using All.In.One.Services;

namespace All.In.One
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AppBuilder.Instance
                .PersistSessionToRedis()
                .AddCloudFoundryActuators()
                .AddCloudFoundryMetricsForwarder()
                .AddConsoleSerilogLogging(true)
                .AddDynamicHttpHandler<AppInfoApiHandler>()
                .AddDynamicHttpHandler<SimpleAuthHandler>()
                .ConfigureServices((hostBuilder, services) =>
                {
                    services.AddTransient<IHealthContributor, MyCustomHealthContributor>();
                    services.AddTransient<ICalcService, CalcService>();
                })
                .AddDefaultConfigurations(jsonSettingsOptional: true, yamlSettingsOptional: true)
                .AddConfigServer()
                .Build()
                .Start();
        }

        protected void Application_Stop()
        {
            AppBuilder.Instance.Stop();
        }
    }

    public class SessionControllerHandler : HttpControllerHandler, IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData)
            : base(routeData)
        { }
    }

    public class SessionHttpControllerRouteHandler : HttpControllerRouteHandler
    {
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionControllerHandler(requestContext.RouteData);
        }
    }
}
