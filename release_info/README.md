#### 2.0.1-rc1
- Using steeltoe prerelease version 2.4.1-ci1602 from https://www.myget.org/F/steeltoedev/api/v3/index.json for the fix of https://github.com/SteeltoeOSS/steeltoe/issues/161
- Note: Make sure to add the above myget feed for Steeltoe Dev

#### 2.0.0
- Package and Samples updgraded to use Steeltoe version 2.4.0

#### 2.0.0-rc3
- Samples added for all implementations, under `/samples` folder
- Logging, Actuator and Redis Package - issue (due to missing config package dependency) fix
- Logging - Serilog.Sinks.Debug missing dependency issue fix

#### 2.0.0-rc2
- Added Yaml configuration support
- Added ability to inject dynamic http handlers, refer to readme section [Base feature (Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-feature-dynamic-handlers) for more details

#### 2.0.0-rc1
- Initial pre-release version (stable)
- Depends on Steeltoe prerelease version 2.4.0-rc1
- Note: Endpoint Actuators are available only for health and info due to a bug (https://github.com/SteeltoeOSS/steeltoe/issues/161). Will not be able to enable other endpoints until the issue is fixed.

#### Issues - Open
1. Unable to configure log levels dynamically via AppsManager

#### Issues - Closed
##### v2.0.1-rc1
1. Endpoint Actuators are available only for health and info due to a bug (https://github.com/SteeltoeOSS/steeltoe/issues/161)

#### Reference/Usage Guide
https://www.initpals.com/pcf/move-your-asp-net-workloads-to-pivotal-platform-pas-cloudfoundry/
