using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;
using System;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDistributedTracing(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? throw new ArgumentNullException(nameof(ILoggerFactory));

            services.AddSingleton<ITracingOptions>((p) =>
            {
                return new TracingOptions(string.Empty, configuration);
            });

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IDiagnosticObserver, OutboundRequestObserver>());
            services.TryAddSingleton<ITracing, OpenCensusTracing>();
            services.TryAddSingleton<IDynamicMessageProcessor, TracingLogProcessor>();
            services.TryAddSingleton(loggerFactory.CreateLogger<DiagnosticsManager>());
            services.TryAddSingleton<IDiagnosticsManager, DiagnosticsManager>();
        }
    }
}
