### Quickly replatform a ASP.Net full framework app to Pivotal Platform (PAS), supports to implement few critical of 12/15 factors

[![Build Status](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_apis/build/status/alfusinigoj.pivotal_cloudfoundry_replatform_bootstrap?branchName=master)](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_build/latest?definitionId=2&branchName=master)

##### Configuration
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Configuration.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Configuration.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Configuration)

##### Logging
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Logging.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Logging.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Logging)

##### Actuators
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Actuators.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Actuators.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Actuators)

##### Redis.Session
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Redis.Session.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Redis.Session.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Redis.Session)

##### Base/IoC
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Base.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Base.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Base)

##### Supported ASP.Net apps
- WebAPI
- MVC
- WebForms
- Other types like (.asmx, .ashx)
- All the above with Unity
- All the above with Autofac

##### Salient features
- One stop package/reference code for replatforming ASP.NET apps to Pivotal Platform (PAS)
- Uses https://steeltoe.io 2.x versions for Configuration, Dynamic Logging, Connector, CF Actuators and CF Metrics Forwarder
- Supports distributed and structured logging, enhanced with Serilog
- Supports IoC using Autofac and Unity apart from native Microsoft ServiceCollection
- Supports multiple config sources (Web.config, appsettings.json, appsettings.{environment}.json, environment variables, vcap services and config server)
- Supports configuration placeholder resolving using pattern matching like, `${variable_name}`. Refer https://steeltoe.io/app-configuration/docs fro more details
- Injects all above configuration into WebConfiguration (appsettings, connection strings and providers) at runtime so as to be used by legacy libraries relying on.
- Helps in getting an ASP.Net app to Pivotal Platform (PAS) or any Cloud Foundry platform within short span of time
- Supports Session persistence to Redis
- Explicit access to any of the injected dependencies across your code. For e.g to access `IConfiguration` you can access it using `DependencyContainer.GetService<IConfiguration>()`. You can also access them via constructor injection which absolutely depends on the IoC framework and application.

##### Packages
 - Externalizing Configuration - https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration
 - Cloud Native Logging - https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging
 - Spring Boot Actuators - https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators
 - Externalizing Session - https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session
 - Base package supporting IoC frameworks - https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base

##### Prerequisites
- Make sure your application is upgraded to ASP.NET framework 4.6.2 or above
- Pivotal Platform (PAS) with `hwc_buildpack` buildpack and `windows` stack

##### Externalizing Configuration
- Make use of the cf extension buildpack https://github.com/cloudfoundry-community/web-config-transform-buildpack which performs token replacement, transformation, etc. during build staging itself. It requires zero code change. Extension buildpacks are preferred way to do, as they do not need any code changes at all. This buildpack is available in pivnet https://network.pivotal.io/products/buildpack-extensions/. For more details, refer to https://docs.pivotal.io/platform/application-service/2-7/buildpacks/hwc/web-config-transform-buildpack.html. You can also find the sampe app at https://github.com/cloudfoundry-community/webconfig-example-app. However, if you are unable to get the buildpack in the platform, you can continue with this package.
- Install package https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base
- Environment variable `ASPNETCORE_ENVIRONMENT` to be set
- In `Global.asax.cs`, add code as below under `Application_Start`

```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...
    
    protected void Application_Start()
    {
    	...
        AppBuilder.Instance
                .AddDefaultConfigurations() 
                .AddConfigServer() //For config server 
                .Build()
                .Start();
    	...
    }
```
- `AddDefaultConfigurations()` have optional parameters
	- `jsonSettingsOptional` if appsettings.json is must
	- `environment` to override environment variable `ASPNETCORE_ENVIRONMENT`

- `AddConfigServer()` have optional parameters
	- `environment` to override environment variable `ASPNETCORE_ENVIRONMENT`
	- `configServerLogger` if a seperate logger factory to be provided

- The order is important here. In this case, configurations from config server take over others
- All default configurations will be added internally, but can always override using json or environment variables as below (if required)

```
{
  "spring": {
    "application": {
      "name": "${vcap:application:name}"
    },
    "cloud": {
      "config": {
        "validate_certificates": false,
        "failFast": false,
        "name": "${vcap:application:name}",
        "env": "${ASPNETCORE_ENVIRONMENT}"
      }
    }
  },
  "AppSettings": {
    "Key1": "value1"
  },
  "ConnectionStrings": {
    "Database1": "connection1"
  },
  "Providers": {
    "Database1": "provider1"
  }
}

```
- Push the app and bind your app to a config server instance and you are good to go
- Instructions to setup config server is available here https://pivotal.io/application-transformation-recipes/app-architecture/setting-up-spring-config-server
- This uses Steeltoe Configurations, to know more about Steeltoe Configuration, go to https://steeltoe.io/app-configuration/get-started
- Order of configuration providers: `web.config, appsettings.json, appsettings.{environment}.json, cups/vcap, config server, environment variables`
- AppSettings and ConnectionString sections in web.config can be overwritten by any of the proceeding configuration sources. This will help in avoiding code changes where application is already using say `ConfigurationManager.AppSettings["foo"]` or `ConfigurationManager.ConnectionStrings["bar"].ConnectionString`
	- For example, say you have an appsetting key named `Foo` to be externalized, you can set an environment variable like `AppSettings:Foo` to overwrite
	- For example, say you have an connection string named `Bar` with Provider to be externalized, you can set an environment variable like `ConnectionString:Bar` and `Providers:Bar` to overwrite it
- You can access `IConfiguration` anywhere in the application using `DependencyContainer.GetService<IConfiguration>()` or via constructor injection
 
##### Persist Session to Redis
- Make use of the cf extension buildpack https://github.com/cloudfoundry-community/redis-session-aspnet-buildpack for persisting session to redis. Extension buildpacks are preferred way to do, as they do not need any code changes at all. However, if you are unable to get the buildpack in the platform, you can continue with this package. Refer here https://www.initpals.com/pcf/cf-buildpack-for-asp-net-apps-to-use-redis-as-session-store-no-code-required/ for more details.
- Install package https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base
- In `Global.asax.cs` and add code as below under `Application_Start`
    
```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...
    
    protected void Application_Start()
    {
    	...
        AppBuilder.Instance
                .PersistSessionToRedis()
                .Build()
                .Start();
    	...
    }
```
- The package will setup `sessionState` section automatically, but will leave the keys under `machineKey` section with a place holder, as below.

```
	<system.web>
		<machineKey validationKey="{Validation Key}" decryptionKey="{Decryption Key} validation="SHA1" decryption="AES" />
		<sessionState mode="Custom" customProvider="RedisSessionStateStore">
			<providers>
			<add name="RedisSessionStateStore" type="Microsoft.Web.Redis.RedisSessionStateProvider" settingsClassName="PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.RedisConnectionHelper" settingsMethodName="GetConnectionString" />
			</providers>
		</sessionState>
	</system.web>

```

- Make sure to generate the machine key section using https://www.developerfusion.com/tools/generatemachinekey and replace the validation and decryption key in the appropriate place holders in web.config. This is a one time activity.
- Push the app and bind your app to a redis instance and you are good to go
- This uses Steeltoe Connector for Redis, to know more about Steeltoe Connectors, go to https://steeltoe.io/service-connectors/get-started

##### Enabling Cloud Foundry Actuators and Metrics Forwarders
- Install package https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base
- In `Global.asax.cs`, add code as below under `Application_Start`

```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...
    
    protected void Application_Start()
    {
    	...
        AppBuilder.Instance
                .AddCloudFoundryActuators()
    			.AddCloudFoundryMetricsForwarder()
                .Build()
                .Start();
    	...
    }
```

- `AddCloudFoundryActuators()` have optional parameter
	- `basePath` to use in case if the app uses context routing
- In `Global.asax.cs`, add code as below under `Application_End`
 ```
    AppBuilder.Instance.Stop();
 ```
- All default configurations will be added internally, but can always override using json or environment variables as below (if required)

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Steeltoe": "Warning",
      "Pivotal": "Warning",
      "System": "Warning",
      "Microsoft": "Warning"
    },
    "Console":
    {
      "IncludeScopes": true
    }
  }
  "management": {
    "endpoints": {
      "path": "/cloudfoundryapplication",
      "cloudfoundry": {
        "validateCertificates": false
      }
    },
    "metrics": {
      "exporter": {
        "cloudfoundry": {
          "validateCertificates": false
        }
      }
    }
  }
}
```
- Push the app, you will be able to see actuators enabled (health, info, etc.) in Apps Manager, but endpoint actuators are limited to `/actuator/health` and `/actuator/info`. Refer to release notes https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/tree/master/release_info for more details.
- This uses Steeltoe Management, to know more about Steeltoe Management, go to https://steeltoe.io/cloud-management/get-started

##### Enable Cloud Native Logging
- Install package https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base
- In `Global.asax.cs`, add code as below under `Application_Start`

```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...
    
    protected void Application_Start()
    {
    	...
        AppBuilder.Instance
                .AddConsoleSerilogLogging()
                .Build()
                .Start();
    	...
    }
```

- `AddConsoleSerilogLogging()` have optional parameter
	- `includeDistributedTracing` if you want to enable distributed tracing logging.To know more about distributed tracing refer to https://www.initpals.com/net-core/asp-net-core-distributed-tracing-using-steeltoe/
- All default configurations will be added internally, but can always override using json or environment variables as below (if required)

```
    {
    	"management": {
    	"tracing": {
    		"AlwaysSample": true,
    		"UseShortTraceIds": false,
    		"EgressIgnorePattern": "/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*"
    	}
    	},
    	"Serilog": {
    	"MinimumLevel": {
    		"Default": "Information",
    		"Override": {
    		"Microsoft": "Warning",
    		"System": "Warning",
    		"Pivotal": "Warning",
    		"Steeltoe": "Warning"
    		}
    	},
    	"WriteTo": [
    		{
    		"Name": "Console",
    		"Args": {
    			"outputTemplate": "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
    		}
    		},
    		{
    		"Name": "Debug",
    		"Args": {
    			"outputTemplate": "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
    		}
    		}
    	]
    	}
    }
```

- This uses Steeltoe Management, to know more about Steeltoe Management, dynamic logging, go to https://steeltoe.io/cloud-management/get-started/logging
- There are few ways you can access logger anywhere in the application 
	- Using the extension method `this.Logger().Log(LogLevel.Debug, "My debug statement");`
	- Get logger from `DependencyContainer`. Sample code below.
	```
		var logger = DependencyContainer.GetService<ILogger<ValuesController>>();
		logger.Log(LogLevel.Trace, "My trace statement");
	```
	- Using Contructor Injection `.ctor(ILogger<ValuesController>> logger)`


##### IoC features
- Install package https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base
- Can add more `Actions` exposed where you can configure; `application configurations`, `inject services` and even modify `logging configurations` as needed.
  
```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...

	protected void Application_Start()
    {
		AppBuilder
			.Instance
			.ConfigureAppConfiguration((hostBuilder, configBuilder) =>
			{
				//Add additional configurations here
			})
			.ConfigureServices((hostBuilder, services) =>
			{
				//Add additional services here
			})
			.ConfigureLogging((hostBuilder, logBuilder) =>
			{
				//configure custome logging here
			}) 
			.Build()
			.Start();
	}
```

- If the application uses `Unity` as its dependency container, you can hook them together as below
      
```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...

	protected void Application_Start()
    {
		AppBuilder
		.Instance
		.ConfigureIoC(
		() => {
			return new Unity.AspNet.WebApi.UnityDependencyResolver(UnityConfig.Container);
		},
		() => {
			return new Unity.AspNet.Mvc.UnityDependencyResolver(UnityConfig.Container);
		},
		(services) => {
			UnityConfig.Container.BuildServiceProvider(services);
			FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
			FilterProviders.Providers.Add(new Unity.AspNet.Mvc.UnityFilterAttributeFilterProvider(UnityConfig.Container));
		})
		.Build()
		.Start();
	}
```
- Sample `UnityConfig` class 
    
```
    using System;
    using Unity;
        
    namespace Foo
    {
        public static class UnityConfig
        {
            private static Lazy<IUnityContainer> container =
                new Lazy<IUnityContainer>(() =>
                {
                    var container = new UnityContainer();
                    RegisterTypes(container);
                    return container;
                });
        
            public static IUnityContainer Container => container.Value;
        
            public static void RegisterTypes(IUnityContainer container)
            {
                // TODO: Register your type's mappings here.
                //container.RegisterType<ITestClass, TestClass>();
            }
        }
    }
```

- If the application uses `Autofac` as its dependency container, you can hook them together as below
  
```
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    ...

	protected void Application_Start()
    {
		AppBuilder
			.Instance
			.ConfigureIoC(
			() => {
				return new AutofacWebApiDependencyResolver(AutofacConfig.Container);
			},
			() => {
				return new AutofacDependencyResolver(AutofacConfig.Container);
			},
			(services) => {
				AutofacConfig.Builder.Populate(services);
			})
			.Build()
			.Start();
	}
```
- Sample `AutofacConfig` class 
        
```
    using Autofac;
    using System;
        
    namespace Foo
    {
        public class AutofacConfig
        {
            private static Lazy<IContainer> container =
                new Lazy<IContainer>(() =>
                {
                    RegisterTypes();
                    return Builder.Build();
                });
        
            public static IContainer Container => container.Value;
        
            public static ContainerBuilder Builder { get; private set; } = new ContainerBuilder();
        
            public static void RegisterTypes()
            {
                // TODO: Register your type's mappings here.
                //Builder.RegisterType<IProductRepository, ProductRepository>();
            }
        }
    }
```

##### Note: Ongoing development packages will be available at feed https://www.myget.org/F/ajaganathan/api/v3/index.json

##### Kindly raise any issues under https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/issues

##### Always welcome, if you are interest in contribution.
