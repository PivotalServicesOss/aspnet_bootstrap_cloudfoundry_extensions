using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public static class AppBuilderExtensions
    {
        internal const string PRINCIPAL_PASSWORD_FROM_CREDHUB = "${vcap:services:credhub:0:credentials:principal_password}";
        public static AppBuilder AddWindowsAuthentication(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["PRINCIPAL_PASSWORD"] = PRINCIPAL_PASSWORD_FROM_CREDHUB;

            var handlers = ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Type>>(instance, "Handlers");

            handlers.Add(typeof(WindowsAuthenticationHandler));

            ReflectionHelper
                 .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                 .Add((builderContext, services) => {
                     //services.AddSingleton<ITicketIssuer, KerberosTicketIssuer>();
                     services.AddSingleton<ITicketIssuer, TestTicketIssuer>();
                     services.AddSingleton<ISpnegoAuthenticator, SpnegoAuthenticator>();
                     services.AddSingleton<ICookieAuthenticator, CookieAuthenticator>();
                 });

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }
    }
}
