using System;
using System.Web.UI;
using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;

namespace WebForm
{
    public partial class _Default : Page
    {
        private readonly ILogger<_Default> logger;

        public _Default()
        {
            this.logger = DependencyContainer.GetService<ILogger<_Default>>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            logger.LogInformation("Explicitly pulled from container logger: I am currently in _Default-Page_Load() method");
            this.Logger().LogInformation("Object extension logger: I am currently in _Default-Page_Load() method");
        }
    }
}