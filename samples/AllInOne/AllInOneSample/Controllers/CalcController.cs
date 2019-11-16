using AllInOneSample.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AllInOneSample.Controllers
{
    public class CalcController : ApiController
    {
        private readonly ICalcService service;
        private readonly ILogger<ConfigController> logger;

        public CalcController(ICalcService service, ILogger<ConfigController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        [Route("~/api/calc/{x:int}/{y:int}/sum")]
        [HttpGet]
        public int Sum(int x, int y)
        {
            logger.LogInformation($"Executing sum of {x} and {y}");
            return service.Add(x, y);
        }

        [Route("~/api/calc/{x:int}/{y:int}/diff")]
        [HttpGet]
        public int Diff(int x, int y)
        {
            logger.LogInformation($"Executing diff of {x} and {y}");
            return service.Substract(x, y);
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "api1: sum/{x}/{y}", "api2: diff/{x}/{y}" };
        }
    }
}
