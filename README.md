
[![Build Status](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_apis/build/status/alfusinigoj.pivotal_cloudfoundry_replatform_bootstrap?branchName=master)](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_build/latest?definitionId=2&branchName=master)

###### Configuration
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration)

###### Logging
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging)

###### Actuators
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators)

###### Redis.Session
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session)

###### Base/IoC
[![NuGet](https://img.shields.io/nuget/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base)
[![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base)


#### What is this for?
- Quickly replatform a ASP.Net full framework app to PCF (PAS), supports to implement few critical of 12/15 factors

##### Supported ASP.Net apps
- WebAPI
- MVC
- WebForms
- Other types like (.asmx, .ashx)
- All the above with Unity
- All the above with Autofac

##### Salient features
- One stop package/reference code
- Use steeltoe.io under the hood for Configuration, Dynamic Logging, Connector, CF Actuators and CF Metrics Forwarder
- Supports distributed and structured logging, enhanced with Serilog
- Supports IoC using Autofac and Unity apart from native Microsoft ServiceCollection
- Supports multiple config sources (Web.config, appsettings.json, appsettings.{environment}.json, environment variables, vcap services and config server)
- Supports configuration placeholder resolving using pattern matching like `${variable_name}`
- Injects all above configuration into WebConfiguration (appsettings, connection strings and providers) at runtime so as to be used by legacy libraries relying on.
- Helps in getting an ASP.Net app to Pivotal Platform (PAS) or any Cloud Foundry platform within short span of time
- Supports Session persistence to Redis

###### Packages
 - Externalizing Configuration - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration
 - Cloud Native Logging - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
 - Spring Boot Actuators - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
 - Externalizing Session - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session
 - Base package supporting IoC frameworks - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

###### Prerequisites
- Make sure your application is upgraded to ASP.NET framework 4.6.2 or above
- Pivotal Platform (PAS) with `hwc_buildpack` buildpack and `windows` stack

####### Externalizing Configuration
- Make use of the cf extension buildpack https://github.com/cloudfoundry-community/web-config-transform-buildpack which performs token replaccement, transformation, etc. Extension buildpacks are preferred way to do, as they do not need any code changes at all. This buildpack is available in pivnet https://network.pivotal.io/products/buildpack-extensions/
- Install package https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
- Environment variable `ASPNETCORE_ENVIRONMENT` to be set
- In `Global.asax.cs` and add code as below under `Application_Start`

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
				"failFast": true,
				"name": "${vcap:application:name}"
				"env": "${ASPNETCORE_ENVIRONMENT}"
			  }
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
- AppSettings and ConnectionString sections in web.config can be overwritten by any of the proceeding configuration sources
	- For example, say you have an appsetting key named `Foo` to be externalized, you can set an environment variable like `AppSettings:Foo` to overwrite
	- For example, say you have an connection string named `Bar` with Provider to be externalized, you can set an environment variable like `ConnectionString:Bar` and `Providers:Bar` to overwrite it

####### Persist Session to Redis
- Make use of the cf extension buildpack https://github.com/cloudfoundry-community/redis-session-aspnet-buildpack for persisting session to redis. Extension buildpacks are preferred way to do, as they do not need any code changes at all.
- Install package https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

```- In `Global.asax.cs` and add code as below under `Application_Start`

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

- Push the app and bind your app to a redis instance and you are good to go
- This uses Steeltoe Connector for Redis, to know more about Steeltoe Connectors, go to https://steeltoe.io/service-connectors/get-started

####### Enabling CLoud Foundry Actuators and Metrics Forwarders
- Install package https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators, which will add its dependency packages including https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
- In `Global.asax.cs` and add code as below under `Application_Start`

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

- All default configurations will be added internally, but can always override using json or environment variables as below (if required)

```
	{
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
- Push the app and bind your app to a redis instance and you are good to go



  ```
  AppBuilder
	.Instance
    .PersistSessionToRedis() //For externalizing session to Redis, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session`
	.AddDynamicConsoleSerilogLogging(includeCorrelation:true) //For Cloud Native logging, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging`
    .AddDefaultConfigurations(jsonSettingsOptional:true, environment:"Production") //Adds Json, Environment Variables and VCAP Services as configuration sources, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration`
	.AddConfigServer(environment:"Production") //Add Config Server as a configuration source, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration` 
	.AddCloudFoundryActuators() //Adds spring cloud actuators, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators` 
	.AddCloudFoundryMetricsForwarder() //Adds metrics forwarder, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators` 
	.Build()
    .Start();
  ```
- Option to add more  `Actions` exposed where you can configure; application configurations, inject services and even modify logging configurations, as needed.
  
  ```
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
  ```

- Option to configure Ioc using Unity
  
  ```
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
  ```

- Option to configure Ioc using Unity
  
  ```
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
  ```
- Configuration details are available under readme.txt under each project (refer to the source repository, https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap). However, the readme file will be downloaded with the package
- Navigate to `Global.asax.cs` and paste the below code under `Application_Stop`
  ```
    AppBuilder.Instance.Stop();
  ```

##### Note: Ongoing development packages will be available at https://www.myget.org/feed/ajaganathan/package/nuget/<PackageId>
