using Microsoft.Extensions.DependencyInjection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Steeltoe.Common.HealthChecks;
using PivotalServices.AspNet.Bootstrap.Extensions;

namespace ActuatorSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppBuilder.Instance
                    .AddCloudFoundryActuators(basePath: null)
                    .AddCloudFoundryMetricsForwarder()
                    .ConfigureServices((hostBuilder, services) =>
                    {
                        services.AddTransient<IHealthContributor, MyCustomHealthContributor>();
                    })
                    .Build()
                    .Start();
        }

        protected void Application_Stop()
        {
            AppBuilder.Instance.Stop();
        }
    }
}
