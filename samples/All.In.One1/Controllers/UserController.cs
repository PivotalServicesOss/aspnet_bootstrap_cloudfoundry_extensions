using All.In.One.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace All.In.One.Controllers
{
    public class UserController : ApiController
    {
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            //Is set in Home page
            var user = HttpContext.Current.Session["UserName"];

            logger.LogInformation($"Currently logged in user (retrieved from session) is, {user}");
            return $"Currently logged in user (retrieved from session) is, {user}";
        }
    }
}
