using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AllInOneSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            Session["UserName"] = User.Identity.Name;

            logger.LogInformation("Construction injection logger: I am currently in HomeController-Index() method");
            this.Logger().LogInformation("Object extension logger: I am currently in HomeController-Index() method");

            var explicitLogger = DependencyContainer.GetService<ILogger<HomeController>>();
            explicitLogger.LogInformation("Explicitly pulled from container logger: I am currently in HomeController-Index() method");

            return View();
        }
    }
}
