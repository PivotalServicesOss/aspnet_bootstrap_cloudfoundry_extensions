using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Ioc
{
    public class WebDependencyResolver : IDependencyResolver
    {
        private IServiceProvider serviceProvider;

        public WebDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDependencyScope BeginScope()
        {
            return new WebDependencyResolver(serviceProvider.CreateScope().ServiceProvider);
        }

        public object GetService(Type serviceType)
        {
            return serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return serviceProvider.GetServices(serviceType);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            serviceProvider = null;
        }

    }
}
