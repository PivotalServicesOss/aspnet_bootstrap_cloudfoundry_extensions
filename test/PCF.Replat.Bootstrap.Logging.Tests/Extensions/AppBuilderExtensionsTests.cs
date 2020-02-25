using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using System.Linq;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PCF.Replat.Bootstrap.Logging.Tests.Extensions
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_AddConsoleSerilogLogging_WithoutCorrelation()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ConfigureLoggingDelegatesProxy.Clear();
            AppBuilder.Instance.AddConsoleSerilogLogging();

            Assert.Single(TestProxy.ConfigureLoggingDelegatesProxy);
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Contains(TestProxy.HandlersProxy, h => h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.GlobalErrorHandler");

            Assert.Equal("Information", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Default"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:Microsoft"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:System"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:PivotalServices"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:Steeltoe"]);
            Assert.Equal("Serilog.Sinks.Console", TestProxy.InMemoryConfigStoreProxy["Serilog:Using:0"]);
            Assert.Equal("Serilog.Sinks.Debug", TestProxy.InMemoryConfigStoreProxy["Serilog:Using:1"]);
            Assert.Equal("Console", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:0:Name"]);
            Assert.Equal("Debug", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:1:Name"]);
            Assert.Equal("[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:0:Args:outputTemplate"]);
            Assert.Equal("[{Level}]RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:1:Args:outputTemplate"]);
        }

        [Fact]
        public void Test_AddConsoleSerilogLogging_WithCorrelation()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ConfigureLoggingDelegatesProxy.Clear();

            AppBuilder.Instance.AddConsoleSerilogLogging(true);

            Assert.Single(TestProxy.ConfigureLoggingDelegatesProxy);
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Contains(TestProxy.HandlersProxy, h =>h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.GlobalErrorHandler");
            Assert.Contains(TestProxy.HandlersProxy, h =>h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.ScopedLoggingHandler");
            Assert.Contains(TestProxy.HandlersProxy, h =>h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.InboundBeginRequestObserverHandler");
            Assert.Contains(TestProxy.HandlersProxy, h =>h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.InboundEndRequestObserverHandler");
            Assert.Contains(TestProxy.HandlersProxy, h =>h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Handlers.InboundErrorRequestObserverHandler");

            Assert.Equal("Information", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Default"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:Microsoft"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:System"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:Pivotal"]);
            Assert.Equal("Warning", TestProxy.InMemoryConfigStoreProxy["Serilog:MinimumLevel:Override:Steeltoe"]);
            Assert.Equal("Serilog.Sinks.Console", TestProxy.InMemoryConfigStoreProxy["Serilog:Using:0"]);
            Assert.Equal("Serilog.Sinks.Debug", TestProxy.InMemoryConfigStoreProxy["Serilog:Using:1"]);
            Assert.Equal("Console", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:0:Name"]);
            Assert.Equal("Debug", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:1:Name"]);
            Assert.Equal("true", TestProxy.InMemoryConfigStoreProxy["management:tracing:AlwaysSample"]);
            Assert.Equal("false", TestProxy.InMemoryConfigStoreProxy["management:tracing:UseShortTraceIds"]);
            Assert.Equal("/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*", TestProxy.InMemoryConfigStoreProxy["management:tracing:EgressIgnorePattern"]);
            Assert.Equal("[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:0:Args:outputTemplate"]);
            Assert.Equal("[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}", TestProxy.InMemoryConfigStoreProxy["Serilog:WriteTo:1:Args:outputTemplate"]);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
