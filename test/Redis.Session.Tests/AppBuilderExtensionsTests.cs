using PivotalServices.AspNet.Bootstrap.Extensions.Testing;
using Xunit;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Redis.Session.Tests
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_PersistSessionToRedis_AddsDelegteIntoConfigureServicesDelegates()
        {
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            AppBuilder.Instance.PersistSessionToRedis();
            Assert.Equal(2, TestProxy.ConfigureServicesDelegatesProxy.Count);
        }
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
