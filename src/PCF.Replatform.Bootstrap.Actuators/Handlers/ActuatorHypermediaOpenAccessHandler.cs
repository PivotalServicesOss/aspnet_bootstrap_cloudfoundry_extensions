using Microsoft.Extensions.Logging;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.Handler;
using Steeltoe.Management.Endpoint.Hypermedia;
using Steeltoe.Management.Endpoint.Security;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.Handlers
{
    [Obsolete("Once the issue https://github.com/SteeltoeOSS/steeltoe/issues/161 is fixed, this class will be removed")]
    internal class ActuatorHypermediaOpenAccessHandler : ActuatorHypermediaHandler
    {
        public ActuatorHypermediaOpenAccessHandler(ActuatorEndpoint endpoint, IEnumerable<ISecurityService> securityServices, IEnumerable<IManagementOptions> mgmtOptions, ILogger<ActuatorHypermediaHandler> logger = null)
            : base(endpoint, securityServices, mgmtOptions, logger)
        {
        }
        public override async Task<bool> IsAccessAllowed(HttpContextBase context)
        {
            if (context.Request.Path.Contains("/actuator/"))
                return await Task.FromResult(result: true);

            return await base.IsAccessAllowed(context);
        }
    }
}