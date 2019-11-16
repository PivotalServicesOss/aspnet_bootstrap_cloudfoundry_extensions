using Microsoft.Extensions.Logging;
using System.Web.Mvc;
using UnitySample.Services;

namespace UnitySample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICalcService service;
        private readonly ILogger<ValuesController> logger;

        public HomeController(ICalcService service, ILogger<ValuesController> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public ActionResult Index()
        {
            ViewBag.Title = $"Home Page";
            ViewBag.SumValue = $"Result of 10 + 20 = {service.Add(10, 20)}";

            return View();
        }
    }
}
