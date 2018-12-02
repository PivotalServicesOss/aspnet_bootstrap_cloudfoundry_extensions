using Pivotal.CloudFoundry.Replatform.Bootstrap.Base;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(HttpModuleConfig), "ConfigureModules")]
