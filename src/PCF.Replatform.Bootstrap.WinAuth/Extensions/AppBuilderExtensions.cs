using Kerberos.NET;
using Kerberos.NET.Crypto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AuthConstants
    {
        public const string SPNEGO_DEFAULT_SCHEME = "Negotiate";
        public const string PRINCIPAL_PASSWORD_FROM_CREDHUB = "${vcap:services:credhub:0:credentials:principal_password}";
    }

    public static class AppBuilderExtensions
    {
        public static AppBuilder AddWindowsAuthentication(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["PRINCIPAL_PASSWORD"] = AuthConstants.PRINCIPAL_PASSWORD_FROM_CREDHUB;

            var handlers = ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Type>>(instance, "Handlers");

            handlers.Add(typeof(WindowsAuthenticationHandler));

            ReflectionHelper
                 .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                 .Add((builderContext, services) =>
                 {
                     services.AddSingleton<KerberosAuthenticator>((provider) => GetAuthenticator(provider));
                     services.AddSingleton<ITicketIssuer, TestTicketIssuer>();
                     services.AddSingleton<ISpnegoAuthenticator, SpnegoAuthenticator>();
                     services.AddSingleton<ICookieAuthenticator, CookieAuthenticator>();
                 });

            instance.AddDefaultConfigurations();
            instance.AddConfigServer();

            return instance;
        }

        private static KerberosAuthenticator GetAuthenticator(IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            return new KerberosAuthenticator(new KerberosValidator(new KerberosKey(configuration["PRINCIPAL_PASSWORD"])))
            {
                UserNameFormat = UserNameFormat.DownLevelLogonName
            };
        }
    }
}
