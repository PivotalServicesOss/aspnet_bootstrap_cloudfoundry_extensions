using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Spnego
{
    public static class ServiceCollectionExtensions
    {
        private static IServiceCollection AddAuthentication<TOptions, THandler>(this IServiceCollection services, string authenticationScheme, string displayName, Action<TOptions> configureOptions) 
                                                                                        where TOptions : class, new() 
                                                                                        where THandler : class, IAuthenticationHandler
        {
            services.AddTransient<IDataProtectionProvider, EphemeralDataProtectionProvider>();
            services.AddTransient<ISystemClock, SystemClock>();
            services.AddTransient((p)=> { return UrlEncoder.Default; });

            services.Configure(delegate (AuthenticationOptions o)
            {
                o.AddScheme(authenticationScheme, delegate (AuthenticationSchemeBuilder scheme)
                {
                    scheme.HandlerType = typeof(THandler);
                    scheme.DisplayName = displayName;
                });
            });

            if (configureOptions != null)
            {
                services.Configure("Negotiate", configureOptions);
            }

            services.AddTransient<IAuthenticationHandler, THandler>();

            return services;
        }

        public static IServiceCollection AddSpnegoAuthentication<TOptions, THandler>(this IServiceCollection services, string displayName, Action<TOptions> configureOptions)
                                                                                        where TOptions : class, new()
                                                                                        where THandler : class, IAuthenticationHandler
        {
            services.AddAuthentication<TOptions, THandler>("Negotiate", displayName, configureOptions);
            return services;
        }

        public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, string displayName, Action<CookieAuthenticationOptions> configureOptions)
        {
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
            services.AddAuthentication<CookieAuthenticationOptions, CookieAuthenticationHandler>("Cookies", displayName, configureOptions);
            return services;
        }
    }
}