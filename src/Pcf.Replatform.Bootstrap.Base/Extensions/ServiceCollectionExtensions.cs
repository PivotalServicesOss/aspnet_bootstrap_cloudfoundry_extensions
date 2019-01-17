using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDefaultDiagnosticsDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITracingOptions>((p) =>
            {
                return new TracingOptions(string.Empty, configuration);
            });

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, OutboundRequestObserver>());
            services.TryAddSingleton<ITracing, OpenCensusTracing>();
            services.TryAddSingleton<IDynamicMessageProcessor, TracingLogProcessor>();
            services.TryAddSingleton<IDiagnosticsManager>(DiagnosticsManager.Instance);
        }

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
