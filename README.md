### Quickly replatform a ASP.Net full framework app to Pivotal Platform (PAS), supports to implement few critical of 12/15 factors

#### Know more about cloud native factors and make use of this package, read this article, [Move APS.NET workloads to PAS](https://www.initpals.com/pcf/move-your-asp-net-workloads-to-pivotal-platform-pas-cloudfoundry/)

#### IMPORTANT RELEASE NOTES (For existing users)

- [PivotalServices.AspNet.Replatform.Cf.WinAuth](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.WinAuth) package has been renamed to [PivotalServices.AspNet.Auth.Extensions](https://www.nuget.org/packages/PivotalServices.AspNet.Auth.Extensions) and moved to [another github repository](). Older package has been deprecated.
- [PivotalServices.AspNet.Replatform.Cf.Base](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base) package has been renamed to [PivotalServices.AspNet.Bootstrap.Extensions](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions) and moved to [another github repository](). Older package has been deprecated.
- [PivotalServices.AspNet.Replatform.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration) package has been renamed to [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration). Older package has been deprecated.
- [PivotalServices.AspNet.Replatform.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging) package has been renamed to [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging). Older package has been deprecated.
- [PivotalServices.AspNet.Replatform.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators) package has been renamed to [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators). Older package has been deprecated.
- [PivotalServices.AspNet.Replatform.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session) package has been renamed to [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session). Older package has been deprecated.

Build | Configuration | Logging | Actuators | Redis.Session |
--- | --- | --- | --- |--- |
[![Build Status](https://dev.azure.com/ajaganathan-home/pivotal_aspnet_bootstrap_cloudfoundry_extensions/_apis/build/status/alfusinigoj.pivotal_aspnet_bootstrap_cloudfoundry_extensions?branchName=master)](https://dev.azure.com/ajaganathan-home/pivotal_aspnet_bootstrap_cloudfoundry_extensions/_build/latest?definitionId=2&branchName=master) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session) 

### Quick Links
- [Supported ASP.NET apps](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#supported-aspnet-apps)
- [Salient features](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#salient-features)
- [Packages](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#packages)
- [Steps - High level](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#steps---high-level)
- [Prerequisites](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#prerequisites)
- [Externalizing Configuration](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#externalizing-configuration)
- [Persist Session to Redis](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#persist-session-to-redis)
- [Enabling Cloud Foundry Actuators and Metrics Forwarders](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#enabling-cloud-foundry-actuators-and-metrics-forwarders)
- [Enable Cloud Native Logging](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#enable-cloud-native-logging)
- [Sample Implementations](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/tree/master/samples) 

### Supported ASP.NET apps
- WebAPI
- MVC
- WebForms
- Other types like (.asmx, .ashx)
- All the above with Unity
- All the above with Autofac

### Salient features
- One stop package/reference code for replatforming ASP.NET apps to Pivotal Platform (PAS)
- Uses [Steeltoe](https://steeltoe.io) 2.x versions for Configuration, Dynamic Logging, Connector, CF Actuators and CF Metrics Forwarder.
- Supports distributed and structured logging, enhanced with Serilog
- Supports multiple config sources (Web.config, appsettings.json, appsettings.{environment}.json, appsettings.yaml, appsettings.{environment}.yaml, environment variables, vcap services and config server)
- Supports configuration placeholder resolving using pattern matching like, `${variable_name}`. Refer [SteeltoeAppConfiguration](https://steeltoe.io/app-configuration/docs) for more details
- Pull in secrets from credhub with easy placeholder resolvements
- Injects all above configuration into WebConfiguration (appsettings, connection strings and providers) at runtime so as to be used by legacy libraries relying on.
- Helps in getting an ASP.Net app to Pivotal Platform (PAS - Cloud Foundry) within short span of time and effort
- Supports Session persistence to Redis
- Explicit access to any of the injected dependencies across your code. For e.g to access `IConfiguration` you can access it using `DependencyContainer.GetService<IConfiguration>()`. You can also access them via constructor injection which absolutely depends on the IoC framework and application.
- Real samples are available [here](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/tree/master/samples) 

### Packages
- Externalizing Configuration - [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration)
- Cloud Native Logging - [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging)
- Spring Boot Actuators - [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators)
- Externalizing Session - [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session)
 
### Steps - High level
- Install the nuget package based on your need
- Modify `App_Start` and `App_End` in Global.ascx (by following the steps in appropriate sections - [Configuration](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#externalizing-configuration), [Session](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#persist-session-to-redis), [Actuators](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#enabling-cloud-foundry-actuators-and-metrics-forwarders), [Logging](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions#enable-cloud-native-logging), 
[Base(IoC)](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/#base-features-ioc), 
[Base(Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/#base-feature-dynamic-handlers))
- Compile and push the application to Pivotal Platform (PAS)

### Prerequisites
- Make sure your application is upgraded to ASP.NET framework 4.6.2 or above
- Pivotal Platform (PAS) with `hwc_buildpack` buildpack and `windows` stack

### Externalizing Configuration
- Make use of the [web config extension buildpack](https://github.com/cloudfoundry-community/web-config-transform-buildpack) which performs token replacement, transformation, etc. during build staging itself. It requires zero code change. Extension buildpacks are preferred way to do, as they do not need any code changes at all. This buildpack is available in [download from pivnet]( https://network.pivotal.io/products/buildpack-extensions/). For more details, refer to [web-config-transform-buildpack](https://docs.pivotal.io/platform/application-service/2-7/buildpacks/hwc/web-config-transform-buildpack.html). You can also find the sampe app at [Github](https://github.com/cloudfoundry-community/webconfig-example-app). However, if you are unable to get the buildpack in the platform, you can continue with this package.
- Install package [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration)
- Environment variable `ASPNETCORE_ENVIRONMENT` to be set
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddDefaultConfigurations() 
                .AddConfigServer() //If you have config server bounded 
                .Build()
                .Start();
    }
```
- `AddDefaultConfigurations()` have optional parameters
	- `jsonSettingsOptional` if appsettings.json is must
	- `yamlSettingsOptional` if appsettings.yaml is must
	- `environment` to override environment variable `ASPNETCORE_ENVIRONMENT`

- `AddConfigServer()` have optional parameters
	- `environment` to override environment variable `ASPNETCORE_ENVIRONMENT`
	- `configServerLogger` if a seperate logger factory to be provided

- The order is important here. In this case, configurations from config server take over others, also make sure these configuration extensions methods (`AddDefaultConfigurations` and `AddConfigServer`) should preceed all other extensions methods.
- Below is the default configurations will be added internally, but can always override using json or yaml or environment variables as below (if at all required)

```yaml
---
spring:
  application:
    name: "${vcap:application:name}"
  cloud:
    config:
      validate_certificates: false
      failFast: false
      name: "${vcap:application:name}"
      env: "${ASPNETCORE_ENVIRONMENT}"
AppSettings:
  Key1: value1
ConnectionStrings:
  Database1: connection1
Providers:
  Database1: provider1

```
- Push the app and bind your app to a config server instance and you are good to go.
- Instructions to setup config server is available here [setting-up-spring-config-server](https://pivotal.io/application-transformation-recipes/app-architecture/setting-up-spring-config-server)
- This uses Steeltoe Configurations, to know more about Steeltoe Configuration, go to [Steeltoe AppConfiguration](https://steeltoe.io/app-configuration/get-started)
- Order of configuration providers: `web.config, appsettings.json, appsettings.{environment}.json, appsettings.yaml, appsettings.{environment}.yaml, cups/vcap, config server, environment variables`
- AppSettings and ConnectionString sections in web.config can be overwritten by any of the proceeding configuration sources. This will help in avoiding code changes where application is already using say `ConfigurationManager.AppSettings["foo"]` or `ConfigurationManager.ConnectionStrings["bar"].ConnectionString`
	- For example, say you have an appsetting key named `Foo` to be externalized, you can set an environment variable like `AppSettings:Foo` to overwrite
	- For example, say you have an connection string named `Bar` with Provider to be externalized, you can set an environment variable like `ConnectionString:Bar` and `Providers:Bar` to overwrite it
- You can access `IConfiguration` anywhere in the application using `DependencyContainer.GetService<IConfiguration>()` or via constructor injection

##### Using Secrets from Credhub

- If you have secrets in credhub with instance name `mycredhubinstance`, you can easily make use of placeholder resolver, by following the below steps.
	- Make sure the application is binded to the credhub instance, in this case `mycredhubinstance` 
	- Say if you have stored secrets for your database connection string in credhub. e.g. `{["db1username":"foo", "db1password":"bar"]}`.
	- You can easily manipulate the connection string stored in any of the configuration sources (web.config, json, yaml, environment variables and config server). Lets pick environment variable for this example, where your variable looks like `db1ConnStr="Data Source=serverAddress;Initial Catalog=db1;User ID=${vcap:services:credhub:0:credentials:db1username};Password=${vcap:services:credhub:0:credentials:db1password};"`
	- During application start, the place holders will be replaced and ready to be served in the application as needed. In our case, the connection string `db1ConnStr` at runtime looks like `Data Source=serverAddress;Initial Catalog=db1;User ID=foo;Password=bar;`
	- To learn more about managing secrets in credhub, please refer to [managing-secrets-with-credhub](https://docs.pivotal.io/spring-cloud-services/3-1/common/config-server/managing-secrets-with-credhub.html)
 
### Persist Session to Redis
- Make use of the [persist session in redis extension buildpack](https://github.com/cloudfoundry-community/redis-session-aspnet-buildpack) for persisting session to redis. Extension buildpacks are preferred way to do, as they do not need any code changes at all. However, if you are unable to get the buildpack in the platform, you can continue with this package. Refer the article [cf-buildpack-for-asp-net-apps-to-use-redis-as-session-store-no-code-required](https://www.initpals.com/pcf/cf-buildpack-for-asp-net-apps-to-use-redis-as-session-store-no-code-required/) for more details.
- Install package [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session)
- In `Global.asax.cs` and add code as below under `Application_Start`
    
```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .PersistSessionToRedis()
                .Build()
                .Start();
    }
```
- The package will setup `sessionState` section automatically, but will leave the keys under `machineKey` section with a place holder, as below.

```xml
<system.web>
	<machineKey validationKey="{Validation Key}" decryptionKey="{Decryption Key}" validation="SHA1" decryption="AES" />
	<sessionState mode="Custom" customProvider="RedisSessionStateStore">
		<providers>
			<add name="RedisSessionStateStore" type="Microsoft.Web.Redis.RedisSessionStateProvider"     settingsClassName="PivotalServices.AspNet.Bootstrap.Extensions.Cf.Base.RedisConnectionHelper" settingsMethodName="GetConnectionString" />
		</providers>
	</sessionState>
</system.web>
```

- Make sure to generate the machine key section using [generatemachinekey](https://www.developerfusion.com/tools/generatemachinekey) and replace the validation and decryption key in the appropriate place holders in web.config. This is a one time activity.
- Push the app and bind your app to a redis instance and you are good to go
- This uses Steeltoe Connector for Redis, to know more about Steeltoe Connectors, go to [Steeltoe Service COnnectors](https://steeltoe.io/service-connectors/get-started)

### Enabling Cloud Foundry Actuators and Metrics Forwarders
- Install package [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators)
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddCloudFoundryActuators()
    			.AddCloudFoundryMetricsForwarder()
                .Build()
                .Start();
    }
```

- `AddCloudFoundryActuators()` have optional parameter
	- `basePath` to use in case if the app uses context routing
- If you need to inject additional `Health Contributor`, you can create your own implementation of `Steeltoe.Common.HealthChecks.IHealthContributor` and inject them as below. Lets assume that we have a custom health contributor called `MyCustomHealthContributor`.

```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddCloudFoundryActuators()
    			.AddCloudFoundryMetricsForwarder()
				.ConfigureServices((hostBuilder, services) =>
				{
		      			services.AddTransient<IHealthContributor, MyCustomHealthContributor>();
				})
                .Build()
                .Start();
    }
```

- In `Global.asax.cs`, add code as below under `Application_End`

 ```c#
    AppBuilder.Instance.Stop();
 ```
 
- Below is the default configurations will be added internally, but can always override using json or yml or environment variables as below (if at all required)

```yaml
---
Logging:
  LogLevel:
    Default: Information
    Steeltoe: Warning
    Pivotal: Warning
    System: Warning
    Microsoft: Warning
  Console:
    IncludeScopes: true
management:
  endpoints:
    path: "/cloudfoundryapplication" 
    cloudfoundry: 
      validateCertificates: false
  metrics:
    exporter:
      cloudfoundry:
        validateCertificates: false

```
- Push the app, you will be able to see actuators enabled (health, info, etc.) in Apps Manager, but endpoint actuators are limited to `/actuator/health` and `/actuator/info`. Refer to [release notes]( https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/tree/master/release_info) for more details.
- This uses Steeltoe Management, to know more, go to [Steeltoe Management](https://steeltoe.io/cloud-management/get-started)

### Enable Cloud Native Logging
- Install package [PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging)
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.AspNet.Bootstrap.Extensions
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddConsoleSerilogLogging()
                .Build()
                .Start();
    }
```

- `AddConsoleSerilogLogging()` have an optional parameter
	- `includeDistributedTracing` if you want to enable distributed tracing logging.To know more about distributed tracing refer to the article [asp-net-core-distributed-tracing-using-steeltoe](https://www.initpals.com/net-core/asp-net-core-distributed-tracing-using-steeltoe/)
- Below is the default configurations will be added internally, but can always override using json or yml or environment variables as below (if at all required)

```yaml
---
management:
  tracing:
  AlwaysSample: true
  UseShortTraceIds: false
  EgressIgnorePattern: "/api/v2/spans|/v2/apps/.*/permissions|/eureka/.*|/oauth/.*"
Serilog:
  MinimumLevel:
    Default: Information
    Override:
      Microsoft: Warning
      System: Warning
      Pivotal: Warning
      Steeltoe: Warning
  WriteTo:
   - Name: Console
     Args:
       outputTemplate: "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
   - Name: Debug
     Args:
       outputTemplate: "[{Level}]{CorrelationContext}=> RequestPath:{RequestPath} => {SourceContext} => {Message} {Exception}{NewLine}"
```

- This uses Steeltoe Management Dynamic Loging, to know more, go to [Steeltoe Management Dynamic Loging](https://steeltoe.io/cloud-management/get-started/logging)
- There are few ways you can access logger anywhere in the application 
	- Using the extension method `this.Logger().Log(LogLevel.Debug, "My debug statement");`
	- Get logger from `DependencyContainer`. Sample code below.
	```
		var logger = DependencyContainer.GetService<ILogger<ValuesController>>();
		logger.Log(LogLevel.Trace, "My trace statement");
	```
	- Using Contructor Injection `.ctor(ILogger<ValuesController>> logger)`

### Ongoing development packages in MyGet

Feed | Configuration | Logging | Actuators | Redis.Session | Windows Authentication | Base |
--- | --- | --- | --- |--- | --- | -- |
[V3](https://www.myget.org/F/ajaganathan/api/v3/index.json) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Configuration) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Actuators) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.WinAuth.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.WinAuth) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Base.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Bootstrap.Extensions.Cf.Base) 

### Issues
- Kindly raise any issues under [GitHub Issues](https://github.com/alfusinigoj/pivotal_aspnet_bootstrap_cloudfoundry_extensions/issues)

### Contributions are welcome!
