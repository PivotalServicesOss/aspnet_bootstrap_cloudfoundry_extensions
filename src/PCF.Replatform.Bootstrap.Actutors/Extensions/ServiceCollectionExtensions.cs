using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Actuators
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services)
        {
            var allTypes = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                           from type in assembly.GetTypes()
                           select type;

            var controllerTypes = allTypes.Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)
                                    .Where(type => typeof(IController).IsAssignableFrom(type)
                                    || type.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase));

            foreach (var type in controllerTypes)
                services.AddTransient(type);

            return services;
        }

    }
}
