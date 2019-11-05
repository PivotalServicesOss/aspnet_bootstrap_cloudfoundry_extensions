using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Redis.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCF.Replat.Bootstra.Redis.Session.Tests
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_PersistSessionToRedis_AddsDelegteIntoConfigureServicesDelegates()
        {
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            AppBuilder.Instance.PersistSessionToRedis();
            Assert.Single(TestProxy.ConfigureServicesDelegatesProxy);
        }
    }
}
