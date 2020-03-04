using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ILogger<ValuesController> logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            this.logger = logger;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            logger.LogInformation("Construction injection logger: I am currently in ValuesController-Get() method");
            this.Logger().LogInformation("Object extension logger: I am currently in ValuesController-Get() method");

            var explicitLogger = DependencyContainer.GetService<ILogger<HomeController>>();
            explicitLogger.LogInformation("Explicitly pulled from container logger: I am currently in ValuesController-Get() method");

            return new string[] { "value1", "value2" };
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
