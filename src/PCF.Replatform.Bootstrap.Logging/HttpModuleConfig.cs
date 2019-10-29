using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Logging;
using System;
using System.Collections.Generic;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(HttpModuleConfig), "ConfigureModules")]

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Logging
{
    public class HttpModuleConfig
    {
        public static void ConfigureModules()
        {
            foreach (var moduleType in GetModuleTypes())
            {
                DynamicModuleUtility.RegisterModule(moduleType);
            }
        }

        public static IEnumerable<Type> GetModuleTypes()
        {
            return new List<Type>
            {
                typeof(InboundRequestObserverModule),
                typeof(ScopedLoggingModule),
                typeof(GlobalErrorHandlerModule),
                typeof(RequestLoggerModule),
            };
        }
    }
}
