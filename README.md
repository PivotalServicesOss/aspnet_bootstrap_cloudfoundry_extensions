### Quickly replatform a ASP.Net full framework app to Pivotal Platform (PAS), supports to implement few critical of 12/15 factors

#### Know more about cloud native factors and make use of this package, read this article, [Move APS.NET workloads to PAS](https://www.initpals.com/pcf/move-your-asp-net-workloads-to-pivotal-platform-pas-cloudfoundry/)



Build | Configuration | Logging | Actuators | Redis.Session | WindowsAuth | Base |
--- | --- | --- | --- |--- | --- | --- |
[![Build Status](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_apis/build/status/alfusinigoj.pivotal_cloudfoundry_replatform_bootstrap?branchName=master)](https://dev.azure.com/ajaganathan-home/pivotal-cloudfoundry-replatform-bootstrap/_build/latest?definitionId=2&branchName=master) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Configuration.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Logging.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Actuators.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Redis.Session.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.WinAuth.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.WinAuth) | [![NuGet](https://img.shields.io/nuget/v/PivotalServices.AspNet.Replatform.Cf.Base.svg?style=flat-square)](http://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base) 

### Quick Links
- [Supported ASP.NET apps](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#supported-aspnet-apps)
- [Salient features](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#salient-features)
- [Packages](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#packages)
- [Steps - High level](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#steps---high-level)
- [Prerequisites](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#prerequisites)
- [Externalizing Configuration](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#externalizing-configuration)
- [Persist Session to Redis](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#persist-session-to-redis)
- [Enabling Cloud Foundry Actuators and Metrics Forwarders](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#enabling-cloud-foundry-actuators-and-metrics-forwarders)
- [Enable Cloud Native Logging](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#enable-cloud-native-logging)
- [Enable Windows Auth](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#enable-windows-authentication)
- [Base feature (IoC)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-features-ioc)
- [Base feature (Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-feature-dynamic-handlers)
- [Sample Implementations](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/tree/master/samples) 

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
- Supports IoC using Autofac and Unity apart from native Microsoft ServiceCollection
- Provision for injecting http pipeline handlers on the fly
- Supports multiple config sources (Web.config, appsettings.json, appsettings.{environment}.json, appsettings.yaml, appsettings.{environment}.yaml, environment variables, vcap services and config server)
- Supports configuration placeholder resolving using pattern matching like, `${variable_name}`. Refer [SteeltoeAppConfiguration](https://steeltoe.io/app-configuration/docs) for more details
- Pull in secrets from credhub with easy placeholder resolvements
- Injects all above configuration into WebConfiguration (appsettings, connection strings and providers) at runtime so as to be used by legacy libraries relying on.
- Helps in getting an ASP.Net app to Pivotal Platform (PAS - Cloud Foundry) within short span of time and effort
- Supports Session persistence to Redis
- Supports Windows Authentication using Kerberos for ASP.NET Web applications (except WCF)
- Explicit access to any of the injected dependencies across your code. For e.g to access `IConfiguration` you can access it using `DependencyContainer.GetService<IConfiguration>()`. You can also access them via constructor injection which absolutely depends on the IoC framework and application.
- Real samples are available [here](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/tree/master/samples) 
### Packages
- Externalizing Configuration - [PivotalServices.AspNet.Replatform.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration)
- Cloud Native Logging - [PivotalServices.AspNet.Replatform.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging)
- Spring Boot Actuators - [PivotalServices.AspNet.Replatform.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators)
- Externalizing Session - [PivotalServices.AspNet.Replatform.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session)
- Enable Windows Auth - [PivotalServices.AspNet.Replatform.Cf.WinAuth](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.WinAuth)
- Base package - [PivotalServices.AspNet.Replatform.Cf.Base](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base)
 
### Steps - High level
- Install the nuget package based on your need
- Modify `App_Start` and `App_End` in Global.ascx (by following the steps in appropriate sections - [Configuration](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#externalizing-configuration), [Session](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#persist-session-to-redis), [Actuators](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#enabling-cloud-foundry-actuators-and-metrics-forwarders), [Logging](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap#enable-cloud-native-logging), 
[Base(IoC)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-features-ioc), 
[Base(Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-feature-dynamic-handlers))
- Compile and push the application to Pivotal Platform (PAS)

### Prerequisites
- Make sure your application is upgraded to ASP.NET framework 4.6.2 or above
- Pivotal Platform (PAS) with `hwc_buildpack` buildpack and `windows` stack

### Externalizing Configuration
- Make use of the [web config extension buildpack](https://github.com/cloudfoundry-community/web-config-transform-buildpack) which performs token replacement, transformation, etc. during build staging itself. It requires zero code change. Extension buildpacks are preferred way to do, as they do not need any code changes at all. This buildpack is available in [download from pivnet]( https://network.pivotal.io/products/buildpack-extensions/). For more details, refer to [web-config-transform-buildpack](https://docs.pivotal.io/platform/application-service/2-7/buildpacks/hwc/web-config-transform-buildpack.html). You can also find the sampe app at [Github](https://github.com/cloudfoundry-community/webconfig-example-app). However, if you are unable to get the buildpack in the platform, you can continue with this package.
- Install package [PivotalServices.AspNet.Replatform.Cf.Configuration](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Configuration)
- Environment variable `ASPNETCORE_ENVIRONMENT` to be set
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
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
- Install package [PivotalServices.AspNet.Replatform.Cf.Redis.Session](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Redis.Session)
- In `Global.asax.cs` and add code as below under `Application_Start`
    
```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
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
			<add name="RedisSessionStateStore" type="Microsoft.Web.Redis.RedisSessionStateProvider"     settingsClassName="PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.RedisConnectionHelper" settingsMethodName="GetConnectionString" />
		</providers>
	</sessionState>
</system.web>
```

- Make sure to generate the machine key section using [generatemachinekey](https://www.developerfusion.com/tools/generatemachinekey) and replace the validation and decryption key in the appropriate place holders in web.config. This is a one time activity.
- Push the app and bind your app to a redis instance and you are good to go
- This uses Steeltoe Connector for Redis, to know more about Steeltoe Connectors, go to [Steeltoe Service COnnectors](https://steeltoe.io/service-connectors/get-started)

### Enabling Cloud Foundry Actuators and Metrics Forwarders
- Install package [PivotalServices.AspNet.Replatform.Cf.Actuators](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Actuators)
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
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
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
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
- Push the app, you will be able to see actuators enabled (health, info, etc.) in Apps Manager, but endpoint actuators are limited to `/actuator/health` and `/actuator/info`. Refer to [release notes]( https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/tree/master/release_info) for more details.
- This uses Steeltoe Management, to know more, go to [Steeltoe Management](https://steeltoe.io/cloud-management/get-started)

### Enable Cloud Native Logging
- Install package [PivotalServices.AspNet.Replatform.Cf.Logging](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Logging)
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
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

### Enable Windows Authentication
- Uses Kerberos based authentication
- Supports all ASP.NET application types except WCF
- Install package [PivotalServices.AspNet.Replatform.Cf.WinAuth](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.WinAuth)
- In `Global.asax.cs`, add code as below under `Application_Start`

```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddWindowsAuthentication()
                .Build()
                .Start();
    }
``` 
- `AddWindowsAuthentication()` have an optional parameter
	- `principalPassword` can be used incase of pushing the secret from any external sources like vault
- Once you setup the service account and SPN (as mentioned [below](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#create-spn-service-principal-name)), you need to set `PRINCIPAL_PASSWORD` with the service account's password, via environment variable or any other configuration sources you are using for the application (e.g. config server, yaml, json, etc.)
- By default the package tries to resolve from credhub with key `principal_password`. e.g. `{["principal_password":"som secret password"]}`. 

**NOTE: In the case of credhub, key should be in lowercase**

- To whitelist any specific `path` like `actuator/health` or `actuator/info`, you need to set `WHITELIST_PATH_CSV` with the service account's password, via environment variable or any other configuration sources you are using for the application (e.g. config server, yaml, json, etc.)
- By default `/cloudfoundryapplication,/cloudfoundryapplication/,/actuator,/actuator/` are whitelisted

- If not exists already, add the `machineKey` section to `web.config` as below. You can generate a new one from [Developer Fusion](https://www.developerfusion.com/tools/generatemachinekey). This is for data protection purposes.

```
    <machineKey validationKey="B2FFA07BEA941CBFD2F2450A5BE4D8F6ABFFE624F3DBB35BC589D34C5647F65235634AEC71B5C1E2453BE8D466B6818A9438AC2FFE0C09024052FFF27C85EB3C" 
            decryptionKey="4AFFE5CFAE4F97BFAE7736E5A6B85E921EF209FA84F4BC665993E72393B080DC" validation="SHA1" decryption="AES" />
```

**NOTE: The skeleton of the machine key section will be added while installing the package**

- Add the application's url to trusted sites. If your application's url is `http://foo.bar`, add `http://foo.bar` into trusted sites.

**NOTE: Make sure you are browsing the application from a domain joined computer (same domain where the SPN is created)**

### Create SPN (Service Principal Name)

**NOTE: This is mandate for front end browser applications, but for services it is not required. In other words, if you want to access your application via browser, you need to have the SPN created, as mentioned below.**

Identify the service account for which the application should be running under (imagine as your application running in IIS on an APP POOL, under a service account). If your application's url is `http://foo.bar`, then you have to create a SPN for the service account as `http/foo.bar`

Command Syntax(using the above sample SPN):

```text
SetSpn -S http/foo.bar <domain\service_account_name>
```
To check to see which SPNs are currently registered with your service account, run the following command:

```text
SetSpn -L <domain\service_account_name>
```

**NOTE: You should have elevated privileges to execute the above command. Eventually this should be executed part of the deployment pipeline, for each application**

### Base features (Ioc)
- Install package [PivotalServices.AspNet.Replatform.Cf.Base](https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.Base)
- Can add more `Actions` exposed where you can configure; `application configurations`, `inject services` and even modify `logging configurations` as needed.
  
```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

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
      
```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

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
    
```c#
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
  
```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

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
        
```c#
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
#### Base feature (Dynamic Handlers)
- Provision to inject any custom http handler using an implementation of abstract `DynamicHttpHandlerBase`
- Below is a sample api handler `FooHandler` below which responds to a `GET` operation with request path `/foo`. 

```c#
    using Microsoft.Extensions.Logging;
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers;
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
    using System.Web;

    namespace Bar
    {
        public class FooHandler : DynamicHttpHandlerBase
		{
			public FooHandler(ILogger<FooHandler> logger)
				: base(logger)
			{
			}

			public override string Path => "/foo";

			public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.PostAuthorizeRequestAsync;

			public override void HandleRequest(HttpContextBase context)
			{
				switch (context.Request.HttpMethod)
				{
					case "GET":
						PerformGet(context);
						break;
					default:
						logger.LogWarning($"No action found for method {context.Request.HttpMethod}");
						break;
				}
			}

			private void PerformGet(HttpContextBase context)
			{
				context.Response.Headers.Set("Content-Type", "application/json");
				context.Response.Write(new { Name = "FooHandler", Method = "GET" });
			}
		}
    }
```
- Override method `IsEnabledAsync` Default is `true`, but access can be overriden based on permissions here
- Override method `ContinueNextAsync` Should continue processing the request after this handler, default is `false`
- Override property `Path`, request path to which the handler to respond
- Override property `ApplicationEvent`, what kind of application event to handle

- Inject the above handler into the pipeline, as in the code below

```c#
    using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
	using Bar;
    
    protected void Application_Start()
    {
        AppBuilder.Instance
                .AddDynamicHttpHandler<FooHandler>()
                .Build()
                .Start();
    }
```

### Ongoing development packages in MyGet

Feed | Configuration | Logging | Actuators | Redis.Session | Windows Authentication | Base |
--- | --- | --- | --- |--- | --- | -- |
[V3](https://www.myget.org/F/ajaganathan/api/v3/index.json) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Configuration.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Configuration) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Logging.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Logging) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Actuators.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Actuators) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Redis.Session.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Redis.Session) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.WinAuth.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.WinAuth) | [![MyGet](https://img.shields.io/myget/ajaganathan/v/PivotalServices.AspNet.Replatform.Cf.Base.svg?style=flat-square)](https://www.myget.org/feed/ajaganathan/package/nuget/PivotalServices.AspNet.Replatform.Cf.Base) 

### Issues
- Kindly raise any issues under [GitHub Issues](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/issues)

### Contributions are welcome!
