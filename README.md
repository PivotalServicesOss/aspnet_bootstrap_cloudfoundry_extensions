#### What is this for?
- Quickly replatform a ASP.Net full framework app to PCF (PAS)
##### Supported ASP.Net apps
- WebAPI
- MVC
- WebForms
- Other types like (.asmx, .ashx)
- All the above with Unity
- All the above with Autofac
##### Not supported (as of now)
- Owin
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
- appsettings.json templates are available part of the package
- More importantly, adds few of the most important/critical factors (Logs, Config, Process. Concurrency, Admin process)
- Can be extended as we learn
##### Where can I find the samples?
- https://github.com/alfusinigoj/pcf_replatform_bootstrap_samples
##### General Instructions
###### Create a local nuget package (optional to publish as a public package)
- Navigate to the source folder `..\src\Pcf.Replatform.Bootstrap.Base`
- Modify the batch file `nugetpublish.bat` with the correct *local repository folder* of yours
- Execute the batch file `nugetpublish.bat` with argument as the minimum version number. For e.g the highlighted portion is the minimum version number for the package *mynugetpackage 1.1.`2`*
- Your package will be created under your local repo as `pivotal.cloudfoundry.replatform.bootstrap.base`

###### How to use the package
- Make sure your application is upgraded to ASP.NET framework 4.7.1 or above
- Make sure you have added the local repo folder as one of the nuget sources (use `visual studio` or modify `nuget.config` directly)
- Open nuget package manager and install the package `Pivotal.CloudFoundry.Replatform.Bootstrap.Base`, make sure you install the latest versions
- Make sure the installation is completed successfully
- Now you will see two files `appsettings_template.json` and `appsettings.development_template.json` downloaded part of the installation. Open these files are replace the placeholders `<place holder>` as necessary. Rename those files to `appsettings.json` and `appsettings.development.json` respectively. Note: The environment, (here development) is pulled from the environment variable value `ASPNET_ENVIRONMENT`. Later you can remove the unused sections, after going through all the steps below.
- If you are using session,
  - Once the package is completely installed, the `web.config` file will be modified automatically, so as to persist session to redis. You can see the  sections `machineKey` and `sessionState` under section `system.web` in the web.config file. Make sure you have only one `sessionState` section, remove all the others except the one with name `RedisSessionStateStore`. 
  - Create a random `machineKey` section using https://www.developerfusion.com/tools/generatemachinekey and replace accordingly
  - End state looks something like this...
    ```
    <machineKey validationKey="838A7FA9D1747FA388E8F6100CE303116B3AFCA6C8A19955CFC75E2DB2D8938EFBD4575EB94C8F5C2B8874E80B5A49037571A4420BA2CE2A44A13738C45C32F7" decryptionKey="D3A95572AC0CB2ED3DA4F2F7DAD21CD57684A609A102B8F5CAB47DECB1409FE1" validation="SHA1" decryption="AES"/>
    <sessionState mode="Custom" customProvider="RedisSessionStateStore">
      <providers>
        <add name="RedisSessionStateStore" type="Microsoft.Web.Redis.RedisSessionStateProvider" settingsClassName="Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Helpers.RedisConnectionHelper" settingsMethodName="GetConnectionString" />
      </providers>
    </sessionState>
    ```
- If you are not using session, remove sections `machineKey` and `sessionState` from the `web.config` file
- Resolve any binding redirects conflicts from the web.config file.
- Now, navigate to `global.asax.cs` and paste the below code under `Application_Start`
  ```
  AppBuilder.Instance
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
        //configure custom logging here
    })
    .PersistSessionToRedis()
    .Build()
    .Start()
  ```
- As you see above, there are `Actions` exposed where you can configure; application configurations, inject services and even modify logging configurations, as needed. Please note that if the application is not using session, you can remove `.PersistSessionToRedis()` from the above code.
- With this lines of code, you get..
    - Configuration injections from `Web.config` (sections--> appSettings and connectionStrings), `appsettings.json`, `appsettings.{ENV:ASPNET_ENVIRONMENT}.json`, `environment variables` and `vcap services`. 
    - Default logging configurations using Serilog (Console and Debug)
    - Ability to add additional configuration sources
    - Ability to inject as many as services (Dependency Injection)
- Navigate to `global.asax.cs` and paste the below code under `Application_Stop`
  ```
    AppBuilder.Instance.Stop();
  ```
- **TODO (add futher information...)**

##### In future
- Improve test coverage, currently very minimal level of unit tests written
- Add Owin support
- Add support and configuration templates for service discovery
