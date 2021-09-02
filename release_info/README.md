#### 1.1.3
- Updated to use Steeltoe version 2.5.4, except for management/actuators to 2.5.1
- Microsoft dependencies updated to latest supporting ASP.NET full framework (< 5.0)

#### 1.1.1
- Consumed base 1.1.1 on safe error handling

#### 1.1.0
- Upgraded to use Steeltoe version 2.4.4
- Upgraded to use MicrosoftExtensions version 3.1.7 (LTS)

#### 1.0.0
- Packages and Namespaces renamed/moved to `PivotalServices.AspNet.Bootstrap.Extensions.Cf.*` for consistency purposes
- Removed `PivotalServices.AspNet.Replatform.Cf.WinAuth` and `PivotalServices.AspNet.Replatform.Cf.Base` which are now moved to seperate repositories along with new package names `PivotalServices.AspNet.Bootstrap.Extensions` and `PivotalServices.AspNet.Auth.Extensions` respectively

#### Reference/Usage Guide
https://www.initpals.com/pcf/move-your-asp-net-workloads-to-pivotal-platform-pas-cloudfoundry/

### Deprecated Packages & Versions (PivotalServices.AspNet.Replatform.Cf.*) below - Unlisted as on 09/1/2021

#### 2.2.0
- A package for Windows Authentication using Kerberos is added, including a sample. https://www.nuget.org/packages/PivotalServices.AspNet.Replatform.Cf.WinAuth

#### 2.1.1
- Steeltoe 2.4.2 is consumed (fix for Issue, https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/issues/12)

#### 2.0.1
- Package and Samples updgraded to use Steeltoe version 2.4.1 which resolved the issue https://github.com/SteeltoeOSS/steeltoe/issues/161

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

