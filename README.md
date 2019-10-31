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

###### Currrently available packages
 - Externalizing Configuration - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Configuration
 - Cloud Native Logging - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
 - Spring Boot Actuators - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators
 - Externalizing Session - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session
 - Base package supporting various IoC frameworks - https://www.nuget.org/packages/PivotalServices.CloudFoundry.Replatform.Bootstrap.Base

###### How to use the package
- Make sure your application is upgraded to ASP.NET framework 4.7.1 or above
- Install the package necesary for the need
- Resolve any binding redirects conflicts from the web.config file, incase of any.
- Now, navigate to `Global.asax.cs` and paste the below code under `Application_Start`
  ```
  AppBuilder.Instance
    .PersistSessionToRedis() //For externalizing session
	.
    .Build()
    .Start()
  ```
- As you see above, there are `Actions` exposed where you can configure; application configurations, inject services and even modify logging configurations, as needed. 
- With this lines of code, you get..
    - Configuration injections from `Web.config` (sections--> appSettings and connectionStrings), `appsettings.json`, `appsettings.{ENV:ASPNET_ENVIRONMENT}.json`, `environment variables` and `vcap services`. 
    - Default logging configurations using Serilog (Console and Debug)
    - Ability to add additional configuration sources
    - Ability to inject as many as services (Dependency Injection)
- Navigate to `Global.asax.cs` and paste the below code under `Application_Stop`
  ```
    AppBuilder.Instance.Stop();
  ```

##### In future
- Improve test coverage, currently very minimal level of unit tests written
