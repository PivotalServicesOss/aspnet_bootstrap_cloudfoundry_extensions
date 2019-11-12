using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AppBuilder
    {
        private AppBuilder() { }

        private static Action<IServiceCollection> configureIoC;
        private static bool useCustomIoC;
        private static Func<IDependencyResolver> mvcDependencyResolverFunc;
        private static Func<System.Web.Http.Dependencies.IDependencyResolver> webApiDependencyResolverFunc;

        internal List<Action<HostBuilderContext, IServiceCollection>> ConfigureServicesDelegates = new List<Action<HostBuilderContext, IServiceCollection>>();
        internal List<Action<HostBuilderContext, IConfigurationBuilder>> ConfigureAppConfigurationDelegates = new List<Action<HostBuilderContext, IConfigurationBuilder>>();
        internal List<Action<HostBuilderContext, ILoggingBuilder>> ConfigureLoggingDelegates = new List<Action<HostBuilderContext, ILoggingBuilder>>();
        internal List<IActuator> Actuators = new List<IActuator>();
        internal List<Type> Handlers = new List<Type>();

        public static readonly AppBuilder Instance = new AppBuilder();

        internal Dictionary<string, string> InMemoryConfigStore { get; } = new Dictionary<string, string>();

        public AppBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            ConfigureAppConfigurationDelegates.Add(configureDelegate);
            return Instance;
        }

        public AppBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            ConfigureServicesDelegates.Add(configureDelegate);
            return Instance;
        }

        public AppBuilder ConfigureLogging(Action<HostBuilderContext, ILoggingBuilder> configureDelegate)
        {
            ConfigureLoggingDelegates.Add(configureDelegate);
            return Instance;
        }

        public AppBuilder ConfigureIoC(Func<System.Web.Http.Dependencies.IDependencyResolver> webApiResolverFunc, Func<IDependencyResolver> mvcResolverFunc, Action<IServiceCollection> configureServiceProvider)
        {
            useCustomIoC = true;
            webApiDependencyResolverFunc = webApiResolverFunc;
            mvcDependencyResolverFunc = mvcResolverFunc;
            configureIoC = configureServiceProvider;
            return Instance;
        }

        public AppBuilder AddDynamicHttpHandler<TCustomHandler>() where TCustomHandler : DynamicHttpHandlerBase
        {
            Handlers.Add(typeof(TCustomHandler));
            return Instance;
        }

        public AppBuilder Build()
        {
            foreach (var handler in Handlers)
                ConfigureServicesDelegates.Add((builder, services) => { services.AddSingleton(typeof(IDynamicHttpHandler), handler); });

            AppConfig.Configure(ConfigureAppConfigurationDelegates,
                                ConfigureServicesDelegates,
                                ConfigureLoggingDelegates,
                                configureIoC,
                                InMemoryConfigStore);

            foreach (var actuator in Actuators)
                actuator.Configure();

            if (!useCustomIoC)
                InstallDefaultDependencyResolvers();
            else
                InstallCustomDependencyResolvers();

            var handlers = DependencyContainer.GetService<IEnumerable<IDynamicHttpHandler>>();

            foreach (var handler in handlers)
                DynamicHttpHandlerModule.Handlers.Add(handler);

            return Instance;
        }

        private object IEnumerable<T>()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            foreach (var actuator in Actuators)
                actuator.Start();
        }

        public void Stop()
        {
            foreach (var actuator in Actuators)
                actuator.Stop();
        }

        private void InstallDefaultDependencyResolvers()
        {
            var resolver = new DefaultDependencyResolver(AppConfig.ServiceProvider);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
            DependencyResolver.SetResolver(resolver);
        }

        private void InstallCustomDependencyResolvers()
        {
            GlobalConfiguration.Configuration.DependencyResolver = webApiDependencyResolverFunc?.Invoke();
            DependencyResolver.SetResolver(mvcDependencyResolverFunc?.Invoke());
        }
    }
}
