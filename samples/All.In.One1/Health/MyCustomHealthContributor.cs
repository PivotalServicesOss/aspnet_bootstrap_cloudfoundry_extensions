using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Steeltoe.Common.HealthChecks;

namespace All.In.One.Health
{
    public class MyCustomHealthContributor : IHealthContributor
    {
        public string Id => "MyCustomHealth";

        public HealthCheckResult Health()
        {
            return new HealthCheckResult() { Status = HealthStatus.UP };
        }
    }
}