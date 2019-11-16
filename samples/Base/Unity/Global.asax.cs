using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity.AspNet.Mvc;
using Unity.Microsoft.DependencyInjection;

namespace UnitySample
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
                .ConfigureServices((hostBuilder, services) =>
                {
                    //Inject services using ServiceCollection here (optional)
                })
                .ConfigureIoC(
                () => {
                    return new Unity.AspNet.WebApi.UnityDependencyResolver(UnityConfig.Container);
                },
                () => {
                    return new UnityDependencyResolver(UnityConfig.Container);
                },
                (services) => {
                    UnityConfig.Container.BuildServiceProvider(services);
                    FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
                    FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));
                })
                .Build()
                .Start();
        }
    }
}
