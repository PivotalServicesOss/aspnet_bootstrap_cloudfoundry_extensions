﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
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
    }
}
