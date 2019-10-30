using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using Steeltoe.Extensions.Logging.SerilogDynamicLogger;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
{
    public static class AppBuilderExtensions
    {
        internal static bool IncludeCorrelation;

        public static AppBuilder AddDynamicConsoleSerilogLogging(this AppBuilder instance, bool includeCorrelation = false)
        {
            IncludeCorrelation = includeCorrelation;

            instance.InMemoryConfigStore.Add("Serilog:MinimumLevel:Default", "Information");
            instance.InMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Microsoft", "Warning");
            instance.InMemoryConfigStore.Add("Serilog:MinimumLevel:Override:System", "Warning");
            instance.InMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Pivotal", "Warning");
            instance.InMemoryConfigStore.Add("Serilog:MinimumLevel:Override:Steeltoe", "Warning");

            instance.InMemoryConfigStore.Add("Serilog:Using:0", "Serilog.Sinks.Console");
            instance.InMemoryConfigStore.Add("Serilog:Using:1", "Serilog.Sinks.Debug");

            instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Name", "Console");
            instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Name", "Debug");

            if(includeCorrelation)
            {
                instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
                instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");

                instance.InMemoryConfigStore.Add("management:tracing:AlwaysSample", "true");
                instance.InMemoryConfigStore.Add("management:tracing:UseShortTraceIds", "false");
                instance.InMemoryConfigStore.Add("management:tracing:EgressIgnorePattern", "/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*");
            }
            else
            {
                instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
                instance.InMemoryConfigStore.Add("Serilog:WriteTo:0:Args:outputTemplate", "[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}");
            }

            instance.ConfigureLoggingDelegates.Add((builderContext, loggingBuilder) => {

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
