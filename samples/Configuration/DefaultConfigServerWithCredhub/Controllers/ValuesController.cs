using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace DefaultConfigServerWithCredhub.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IConfiguration configuration;
        private readonly IOptions<CloudFoundryServicesOptions> serviceOptions;
        private readonly ILogger<ValuesController> logger;

        public ValuesController(IConfiguration configuration, IOptions<CloudFoundryServicesOptions> serviceOptions, ILogger<ValuesController> logger)
        {
            this.configuration = configuration;
            this.serviceOptions = serviceOptions;
            this.logger = logger;
        }

        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
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

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
