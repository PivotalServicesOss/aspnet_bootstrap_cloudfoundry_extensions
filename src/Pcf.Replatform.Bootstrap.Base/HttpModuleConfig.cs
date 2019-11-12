using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using System;
using System.Collections.Generic;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(HttpModuleConfig), "ConfigureModules")]

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
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
                typeof(DynamicHttpHandlerModule),
            };
        }
    }
}
