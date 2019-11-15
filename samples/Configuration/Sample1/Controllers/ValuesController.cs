using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace Sample1.Controllers
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

                $"conn1: {ConfigurationManager.ConnectionStrings["conn1"].ConnectionString}",

                $"conn2: {configuration["ConnectionStrings:conn2"]}",

                $"conn3: {ConfigurationManager.ConnectionStrings["conn3"].ConnectionString}",

                $"conn3provider: {ConfigurationManager.ConnectionStrings["conn3"].ProviderName}",

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
