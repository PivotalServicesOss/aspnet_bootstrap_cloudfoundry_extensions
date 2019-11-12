using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Http.Controllers;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            var allTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           select type;

            var controllerTypes = allTypes.Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                                    .Where(type => (typeof(IController).IsAssignableFrom(type) || typeof(IHttpController).IsAssignableFrom(type))
                                    && type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));
            try
            {
                foreach (var type in controllerTypes)
                {
                    if (!services.Any((desc) => desc?.ImplementationType?.Name == type.Name))
                        services.AddTransient(type);
                }
            }
            catch { }
            return services;
        }

    }
}
