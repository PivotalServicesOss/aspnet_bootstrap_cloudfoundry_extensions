using Kerberos.NET;
using Kerberos.NET.Crypto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Reflection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Authentication;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.DataProtection;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public class AuthConstants
    {
        public const string SPNEGO_DEFAULT_SCHEME = "Negotiate";
        public const string AUTH_COOKIE_NM = "AUTH";

        public const string DATA_PROTECTION_KEY_NM = "DATA_PROTECTION_KEY";
        public const string DATA_PROTECTION_PURPOSE_DEFAULT = "${vcap:application:name}";

        public const string PRINCIPAL_PASSWORD_NM = "PRINCIPAL_PASSWORD";
        public const string PRINCIPAL_PASSWORD_FROM_CREDHUB = "${vcap:services:credhub:0:credentials:principal_password}";

        public const string WHITELIST_PATHS_CSV_NM = "WHITELIST_PATH_CSV";
    }

    public static class AppBuilderExtensions
    {
        public static AppBuilder AddWindowsAuthentication(this AppBuilder instance)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore[AuthConstants.PRINCIPAL_PASSWORD_NM] = AuthConstants.PRINCIPAL_PASSWORD_FROM_CREDHUB;
            inMemoryConfigStore[AuthConstants.DATA_PROTECTION_KEY_NM] = AuthConstants.DATA_PROTECTION_PURPOSE_DEFAULT;

            var handlers = ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Type>>(instance, "Handlers");

            handlers.Add(typeof(WindowsAuthenticationHandler));

            ReflectionHelper
                 .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                 .Add((builderContext, services) =>
                 {
                     services.AddSingleton<IDataProtector, MachineKeyDataProtector>();
                     services.AddSingleton((provider) => GetAuthenticator(provider));

                     services.AddSingleton<ITicketIssuer, KerberosTicketIssuer>();

                     services.AddSingleton<ISpnegoAuthenticator, SpnegoAuthenticator>();
                     services.AddSingleton<ICookieAuthenticator, CookieAuthenticator>();
                 });

            instance.AddDefaultConfigurations(jsonSettingsOptional: false);
            instance.AddConfigServer();

            return instance;
        }

        private static KerberosAuthenticator GetAuthenticator(IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            return new KerberosAuthenticator(new KerberosValidator(new KerberosKey(configuration[AuthConstants.PRINCIPAL_PASSWORD_NM])))
            {
                UserNameFormat = UserNameFormat.DownLevelLogonName
            };
        }
    }
}
