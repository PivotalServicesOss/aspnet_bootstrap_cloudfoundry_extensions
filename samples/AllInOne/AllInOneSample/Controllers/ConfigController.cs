using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllInOneSample.Controllers
{
    public class ConfigController : ApiController
    {
        private readonly IConfiguration configuration;
        private readonly IOptions<CloudFoundryServicesOptions> serviceOptions;
        private readonly ILogger<ConfigController> logger;

        public ConfigController(IConfiguration configuration, IOptions<CloudFoundryServicesOptions> serviceOptions, ILogger<ConfigController> logger)
        {
            this.configuration = configuration;
            this.serviceOptions = serviceOptions;
            this.logger = logger;
        }

        public ConfigController(ILogger<ConfigController> logger)
        {
            this.logger = logger;
        }

        public IEnumerable<string> Get()
        {
            logger.LogInformation("Construction injection logger: I am currently in ConfigController-Get() method");
            this.Logger().LogInformation("Object extension logger: I am currently in ConfigController-Get() method");

            var explicitLogger = DependencyContainer.GetService<ILogger<HomeController>>();
            explicitLogger.LogInformation("Explicitly pulled from container logger: I am currently in ConfigController-Get() method");

            var result = new List<string> {
                $"key1: {ConfigurationManager.AppSettings["key1"]}",

                $"key2: {ConfigurationManager.AppSettings["key2"]}",

                $"key3: {configuration["AppSettings:key3"]}",

                $"key4: {configuration["AppSettings:key4"]}",

                $"conn1: {ConfigurationManager.ConnectionStrings["conn1"].ConnectionString}",
                 $"conn1provider: {ConfigurationManager.ConnectionStrings["conn1"].ProviderName}",

                $"conn2: {configuration["ConnectionStrings:conn2"]}",
                 $"conn2provider: {ConfigurationManager.ConnectionStrings["conn2"].ProviderName}",

                $"conn3: {ConfigurationManager.ConnectionStrings["conn3"].ConnectionString}",
                 $"conn3provider: {ConfigurationManager.ConnectionStrings["conn3"].ProviderName}",

                $"conn4: {ConfigurationManager.ConnectionStrings["conn4"].ConnectionString}",
                 $"conn4provider: {ConfigurationManager.ConnectionStrings["conn4"].ProviderName}",

                $"secretConn: {ConfigurationManager.ConnectionStrings["secretConn"].ConnectionString}",

                $"ASPNETCORE_ENVIRONMENT: {configuration["ASPNETCORE_ENVIRONMENT"]}",
            };

            return result;
        }
    }
}
