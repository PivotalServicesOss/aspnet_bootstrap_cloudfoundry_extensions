using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics;
using System;
using System.Web;

//Minimum tests are added
namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Tests.Diagnostics
{
    [TestClass]
    public class ScopedLoggingModuleTests
    {
        [TestMethod]
        public void Test_IsTypeOfHttpModule()
        {
            Assert.IsTrue(new ScopedLoggingModule() is IHttpModule);
        }
    }
}
