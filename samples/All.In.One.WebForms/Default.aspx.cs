using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;

namespace All.In.One.WebForms
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
            lblUser.Text = User.Identity.Name;
            logger.LogInformation("Explicitly pulled from container logger: I am currently in _Default-Page_Load() method");
            this.Logger().LogInformation("Object extension logger: I am currently in _Default-Page_Load() method");
        }
    }
}