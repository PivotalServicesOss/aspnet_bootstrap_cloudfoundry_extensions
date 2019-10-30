using System.Web.Http;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc
{
    public class DependencyContainer
    {
        public static T GetService<T>()
        {
            return (T)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(T));
        }
    }
}
