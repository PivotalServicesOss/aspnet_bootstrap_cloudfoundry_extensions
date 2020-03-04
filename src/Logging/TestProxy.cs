using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging.Testing
{
    public class TestProxy
    {
        /// <summary>
        /// Only for testing purpose
        /// </summary>
        public static void AddDefaultDiagnosticsDependencies(ServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDefaultDiagnosticsDependencies(configuration);
        }
    }
}
