//using System;
//using System.Text.Encodings.Web;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.DataProtection;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection.Extensions;
//using Microsoft.Extensions.Options;
//using PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers;

//namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Spnego
//{
//    public static class ServiceCollectionExtensions
//    {
//        private static IServiceCollection AddAuthentication<TAuthenticator>(this IServiceCollection services, string authenticationScheme, string displayName, Action<TOptions> configureOptions) 
//                                                                                        where TAuthenticator : class, IAuthenticator
//        {
//            //services.AddTransient<IDataProtectionProvider, EphemeralDataProtectionProvider>();
//            //services.AddTransient<ISystemClock, SystemClock>();
//            //services.AddTransient((p)=> { return UrlEncoder.Default; });

//            //services.Configure(delegate (AuthenticationOptions option)
//            //{
//            //    option.AddScheme(authenticationScheme, delegate (AuthenticationSchemeBuilder scheme)
//            //    {
//            //        scheme.HandlerType = typeof(TAuthenticator);
//            //        scheme.DisplayName = displayName;
//            //    });
//            //});

//            services.AddTransient<IAuthenticator, TAuthenticator>();

//            return services;
//        //}

//        internal static IServiceCollection AddSpnegoAuthentication<TOptions, TAuthenticator>(this IServiceCollection services, string displayName, Action<TOptions> configureOptions)
//                                                                                        where TOptions : class, new()
//                                                                                        where TAuthenticator : class, IAuthenticator
//        {
//            services.AddAuthentication<TOptions, TAuthenticator>("Negotiate", displayName, configureOptions);
//            return services;
//        }

//        //public static IServiceCollection AddCookieAuthentication(this IServiceCollection services, string displayName, Action<CookieAuthenticationOptions> configureOptions)
//        //{
//        //    services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
//        //    services.AddAuthentication<CookieAuthenticationOptions, CookieAuthenticationHandler>("Cookies", displayName, configureOptions);
//        //    return services;
//        //}
//    }
//}