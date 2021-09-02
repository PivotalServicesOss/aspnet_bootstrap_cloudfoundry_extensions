﻿using Microsoft.Extensions.Logging;
using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using PivotalServices.AspNet.Bootstrap.Extensions.Ioc;
using System;
using System.Web;

namespace All.In.One.WebForms.Handlers
{
    public class AppInfoApiHandler : DynamicHttpHandlerBase
    {
        public AppInfoApiHandler()
            : base(DependencyContainer.GetService<ILogger<AppInfoApiHandler>>(true))
        {
        }

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.PostAuthorizeRequestAsync;

        public override string Path => "/appinfo";

        public override void HandleRequest(HttpContextBase context)
        {
            switch (context.Request.HttpMethod)
            {
                case "GET":
                    PerformGet(context);
                    break;
                default:
                    logger.LogWarning($"No action found for method {context.Request.HttpMethod}");
                    break;
            }
        }

        private void PerformGet(HttpContextBase context)
        {
            context.Response.Headers.Set("Content-Type", "application/json");
            context.Response.Write(new { Name = "AppInfoApiHandler", Method = "GET", OsVersion = Environment.OSVersion });
        }
    }
}