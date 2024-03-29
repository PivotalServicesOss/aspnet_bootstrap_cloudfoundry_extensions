﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Reflection;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.Placeholder;
using System;
using System.Collections.Generic;
using System.IO;

namespace PivotalServices.AspNet.Bootstrap.Extensions
{
    public static class AppBuilderExtensions
    {
        const string ASPNET_ENV_VAR = "ASPNETCORE_ENVIRONMENT";

        /// <summary>
        /// Adds Json, Environment Variables and VCAP Services as configuration sources
        /// appsettings.{ASPNETCORE_ENVIRONMENT}.* files are optional
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="jsonSettingsOptional"></param>
        /// <param name="yamlSettingsOptional"></param>
        /// <param name="environment"></param>
        public static AppBuilder AddDefaultConfigurations(this AppBuilder instance, bool jsonSettingsOptional = true, bool yamlSettingsOptional = true, string environment = null)
        {
            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IConfigurationBuilder>>>(instance, "ConfigureAppConfigurationDelegates")
                .Add((builderContext, configBuilder) =>
                {
                    configBuilder.SetBasePath(GetContentRoot());
                    configBuilder.AddWebConfiguration();
                    configBuilder.AddJsonFile("appSettings.json", jsonSettingsOptional, false);
                    configBuilder.AddJsonFile($"appSettings.{environment ?? (Environment.GetEnvironmentVariable(ASPNET_ENV_VAR) ?? string.Empty)}.json", true, false);
                    configBuilder.AddYamlFile("appSettings.yaml", yamlSettingsOptional, false);
                    configBuilder.AddYamlFile($"appSettings.{environment ?? (Environment.GetEnvironmentVariable(ASPNET_ENV_VAR) ?? string.Empty)}.yaml", true, false);
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddCloudFoundry();
                    configBuilder.AddPlaceholderResolver();
                });

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                .Add((builderContext, services) =>
                {
                    services.ConfigureCloudFoundryOptions(builderContext.Configuration);
                    WebConfigurationHelper.OverrideWebConfiguration(builderContext.Configuration);
                });

            return instance;
        }

        /// <summary>
        /// Adds spring cloud config server as a configuration source
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="environment"></param>
        /// <param name="configServerLogger"></param>
        /// <returns></returns>
        public static AppBuilder AddConfigServer(this AppBuilder instance, string environment = null, ILoggerFactory configServerLogger = null)
        {
            var inMemoryConfigStore = ReflectionHelper
                .GetNonPublicInstancePropertyValue<Dictionary<string, string>>(instance, "InMemoryConfigStore");

            inMemoryConfigStore["spring:application:name"] = "${vcap:application:name}";
            inMemoryConfigStore["spring:cloud:config:name"] = "${vcap:application:name}";

            if (!string.IsNullOrWhiteSpace(environment))
                inMemoryConfigStore["spring:cloud:config:env"] = environment;
            else if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(ASPNET_ENV_VAR)))
                inMemoryConfigStore["spring:cloud:config:env"] = "${ASPNETCORE_ENVIRONMENT}";
            else
            {
                //do nothing 
            }

            inMemoryConfigStore["spring:cloud:config:validate_certificates"] = "false";
            inMemoryConfigStore["spring:cloud:config:failFast"] = "false";

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IConfigurationBuilder>>>(instance, "ConfigureAppConfigurationDelegates")
                .Add((builderContext, configBuilder) =>
                {
                    configBuilder.AddConfigServer(configServerLogger);
                    configBuilder.AddEnvironmentVariables();
                    configBuilder.AddPlaceholderResolver();
                });

            ReflectionHelper
                .GetNonPublicInstanceFieldValue<List<Action<HostBuilderContext, IServiceCollection>>>(instance, "ConfigureServicesDelegates")
                .Add((builderContext, services) =>
                {
                    services.ConfigureCloudFoundryOptions(builderContext.Configuration);
                    WebConfigurationHelper.OverrideWebConfiguration(builderContext.Configuration);
                });

            return instance;
        }

        public static string GetContentRoot()
        {
            var basePath = (string)AppDomain.CurrentDomain.GetData("APP_CONTEXT_BASE_DIRECTORY") ?? AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(basePath);
        }
    }
}
