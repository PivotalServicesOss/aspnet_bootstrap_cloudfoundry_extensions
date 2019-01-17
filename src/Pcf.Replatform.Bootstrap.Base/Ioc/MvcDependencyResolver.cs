using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Ioc
{
    public class MvcDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider serviceProvider;

        public MvcDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return serviceProvider.GetServices(serviceType);
        }
    }
}
