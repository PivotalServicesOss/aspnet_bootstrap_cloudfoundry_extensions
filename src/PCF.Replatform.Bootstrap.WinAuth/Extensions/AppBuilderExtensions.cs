using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Kerberos;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Options;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Spnego;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        public static AppBuilder AddWindowsAuthentication(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["PRINCIPAL_PASSWORD"] = "";


            var handlers = ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Type>>(instance, "Handlers");

            handlers.Add(typeof(WindowsAuthenticationHandler));


            ReflectionHelper
                 .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                 .Add((builderContext, services) => {
                     services.AddSpnegoAuthentication<SpnegoAuthenticationOptions, SpnegoAuthenticationHandler>(null, null);
                     services.AddCookieAuthentication(null, null);
                     services.AddSingleton<KerberosAuthenticationEvents>();
                 });

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }
    }
}
