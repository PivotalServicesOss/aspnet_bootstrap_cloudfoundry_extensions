using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using All.In.One.WebForms.Handlers;
using All.In.One.WebForms.Health;
using Microsoft.Extensions.DependencyInjection;
using PivotalServices.AspNet.Bootstrap.Extensions;
using Steeltoe.Common.HealthChecks;

namespace All.In.One.WebForms
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
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
}