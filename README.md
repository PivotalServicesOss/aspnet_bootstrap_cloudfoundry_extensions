
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
- Uses steeltoe packages at the most part
- Supports distributed tracing (inbound and outbound)
- Supports structured logging using Serilog
- Supports all steeltoe actuators including metrics actuator
- Supports metrics forwarder
- Supports IoC using Autofac and Unity apart from native IoC
- Supports multiple config sources (Web.config, appsettings.json, environment variables, vcap services and config server)
- Injects all above configuration into WebConfiguration (appsettings, connection strings and providers) at runtime so as to be used by legacy libraries relying on.
- Reduces replatforming effort from days to hours/minutes
- Helps in getting an ASP.Net app to PCF within short time
- Supports Session persistence to Redis with auto update of Web.config during package install
- appsettings.json templates are available in the respective projects for reference or download
- More importantly, adds few of the most important/critical factors (Logs, Config, Process. Concurrency, Admin process)
- Can be extended as we learn

###### Packages
 - Externalizing Configuration - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration
 - Cloud Native Logging - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
 - Spring Boot Actuators - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
 - Externalizing Session - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session
 - Base package supporting various IoC frameworks - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

###### How to use these package
- Make sure your application is upgraded to ASP.NET framework 4.7.1 or above
- Install the package necesary for the need
- Resolve any binding redirects conflicts from the web.config file, incase of any.
- Now, navigate to `Global.asax.cs` and paste the below code under `Application_Start`
  ```
  AppBuilder
	.Instance
    .PersistSessionToRedis() //For externalizing session to Redis, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session`
	.AddDynamicConsoleSerilogLogging(includeCorrelation:true) //For Cloud Native logging, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging`
    .AddDefaultConfigurationProviders(jsonSettingsOptional:true, environment:"Production") //Adds Json, Environment Variables and VCAP Services as configuration sources, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration`
	.AddConfigServer(environment:"Production") //Add Config Server as a configuration source, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration` 
	.AddHealthActuators() //Adds spring cloud actuators, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators` 
	.AddMetricsForwarder() //Adds metrics forwarder, package source `PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators` 
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
- Configuration details are available under each project appsettings.json file (refer to the source repository, https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap)
- Navigate to `Global.asax.cs` and paste the below code under `Application_Stop`
  ```
    AppBuilder.Instance.Stop();
  ```

##### In future
- Improve test coverage, currently very minimal level of unit tests written

##### Note: Development packages will be available at https://www.myget.org/feed/ajaganathan/package/nuget/<PackageId>
