using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;

namespace AutofacSample
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
                        return new AutofacWebApiDependencyResolver(AutofacConfig.Container);
                    },
                    () => {
                        return new AutofacDependencyResolver(AutofacConfig.Container);
                    },
                    (services) => {
                        AutofacConfig.Builder.Populate(services);
                    })
                    .Build()
                    .Start();
        }
    }
}
