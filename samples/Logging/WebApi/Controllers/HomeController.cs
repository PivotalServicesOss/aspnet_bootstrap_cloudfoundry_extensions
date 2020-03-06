using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Cf.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi.Controllers
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

            logger.LogInformation("Construction injection logger: I am currently in HomeController-Index() method");
            this.Logger().LogInformation("Object extension logger: I am currently in HomeController-Index() method");

            var explicitLogger = DependencyContainer.GetService<ILogger<HomeController>>();
            explicitLogger.LogInformation("Explicitly pulled from container logger: I am currently in HomeController-Index() method");

            return View();
        }
    }
}
