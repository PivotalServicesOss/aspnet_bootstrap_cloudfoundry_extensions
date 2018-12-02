using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using System;
using System.Collections.Generic;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base
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
            };
        }
    }
}
