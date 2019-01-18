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
##### Instructions
- TODO
