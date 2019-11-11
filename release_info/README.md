#### 2.0.0-rc12
- Added Yaml configuration support
- Added ability to inject dynamic http handlers, refer to readme section [Base feature (Dynamic Handlers)](https://github.com/alfusinigoj/pivotal_cloudfoundry_replatform_bootstrap/#base-feature-dynamic-handlers) for more details

#### 2.0.0-rc1
- Initial pre-release version (stable)
- Depends on Steeltoe prerelease version 2.4.0-rc1
- Caution: Endpoint Actuators are available only for health and info due to a bug (https://github.com/SteeltoeOSS/steeltoe/issues/161). Will not be able to enable other endpoints until the issue is fixed.
- Known Issue: Unable to configure log levels dynamically via AppsManager