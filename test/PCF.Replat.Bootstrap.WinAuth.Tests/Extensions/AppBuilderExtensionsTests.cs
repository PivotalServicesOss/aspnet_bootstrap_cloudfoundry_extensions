using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Testing;
using Xunit;

namespace PCF.Replat.Bootstrap.Logging.Tests.Extensions
{
    public class AppBuilderExtensionsTests
    {
        [Fact]
        public void Test_AddWindowsAuthDependenciesSuccessfully()
        {
            TestProxy.InMemoryConfigStoreProxy.Clear();
            TestProxy.ConfigureServicesDelegatesProxy.Clear();
            TestProxy.ConfigureAppConfigurationDelegatesProxy.Clear();
            AppBuilder.Instance.AddWindowsAuthentication();

            Assert.Equal(2, TestProxy.ConfigureAppConfigurationDelegatesProxy.Count);
            Assert.Equal(3, TestProxy.ConfigureServicesDelegatesProxy.Count);

            Assert.Contains(TestProxy.HandlersProxy, h => h.FullName == "PivotalServices.CloudFoundry.Replatform.Bootstrap.WinAuth.Handlers.WindowsAuthenticationHandler");

            Assert.Equal("${vcap:services:credhub:0:credentials:principal_password}", TestProxy.InMemoryConfigStoreProxy[AuthConstants.PRINCIPAL_PASSWORD_NM]);
        }
    }
}
