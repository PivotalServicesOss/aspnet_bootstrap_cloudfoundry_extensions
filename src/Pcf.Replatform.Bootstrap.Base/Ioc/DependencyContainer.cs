using System;
using System.Web.Http;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc
{
    public class DependencyContainer
    {
        public static T GetService<T>()
        {
            var service = (T)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(T));

            if (service == null)
                service = AppConfig.GetService<T>();

            if (service == null)
                throw new ApplicationException($"Service of type '{typeof(T)}' not found in container(s). Please check if you have includeded it into dependency container.");

            return service;
        }
    }
}
