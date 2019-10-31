using Steeltoe.Extensions.Logging.SerilogDynamicLogger;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        internal static bool IncludeCorrelation;

        public static AppBuilder AddDynamicConsoleSerilogLogging(this AppBuilder instance, bool includeCorrelation = false)
        {
            IncludeCorrelation = includeCorrelation;

            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstanceFieldValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore.Add("Serilog:MinimumLevel:Default", "Information");
            inMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Microsoft", "Warning");
            inMemoryConfigStore.Add("Serilog:MinimumLevel:Override:System", "Warning");
            inMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Pivotal", "Warning");
            inMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Steeltoe", "Warning");

            inMemoryConfigStore.Add("Serilog:Using:0", "Serilog.Sinks.Console");
            inMemoryConfigStore.Add("Serilog:Using:1", "Serilog.Sinks.Debug");

            inMemoryConfigStore.Add("Serilog:WriteTo:0:Name", "Console");
            inMemoryConfigStore.Add("Serilog:WriteTo:0:Name", "Debug");

            if(includeCorrelation)
            {
                inMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
                inMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");

                inMemoryConfigStore.Add("management:tracing:AlwaysSample", "true");
                inMemoryConfigStore.Add("management:tracing:UseShortTraceIds", "false");
                inMemoryConfigStore.Add("management:tracing:EgressIgnorePattern", "/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*");
            }
            else
            {
                inMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
                inMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
            }

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, ILoggingBuilder>>>(instance, "ConfigureLoggingDelegates")
                    .Add((builderContext, loggingBuilder) => {
                        var loggerConfiguration = new Serilog.LoggerConfiguration();
                        var serilogOptions = new SerilogOptions(builderContext.Configuration);
                        var levelSwitch = new LoggingLevelSwitch(serilogOptions.MinimumLevel.Default);
                        loggerConfiguration.MinimumLevel.ControlledBy(levelSwitch);

                        var logger = loggerConfiguration.ReadFrom.Configuration(builderContext.Configuration)
                        .Enrich.FromLogContext()
                        .Filter.ByExcluding("Contains(@Message, 'cloudfoundryapplication')")
                        .CreateLogger();

                        Log.Logger = logger;

                        loggingBuilder.Services.AddSingleton<IDynamicLoggerProvider>(sp => new SerilogDynamicProvider(sp.GetRequiredService<IConfiguration>(), logger, levelSwitch));
                        loggingBuilder.Services.AddSingleton<ILoggerFactory>(sp => new SerilogDynamicLoggerFactory(sp.GetRequiredService<IDynamicLoggerProvider>()));

                        if (includeCorrelation)
                        {
                            loggingBuilder.Services.AddDefaultDiagnosticsDependencies(builderContext.Configuration);
                        }
            });
            return instance;
        }
    }
}
