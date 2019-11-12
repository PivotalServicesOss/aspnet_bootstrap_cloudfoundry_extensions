using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
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
#pragma warning restore CS0618 // Type or member is obsolete
