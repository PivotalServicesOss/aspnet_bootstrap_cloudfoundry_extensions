using Microsoft.Extensions.Logging;
using Steeltoe.Common.Diagnostics;
using System;
using System.Web;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics
{
    public class RequestLoggerModule : IHttpModule
    {
        ILogger<RequestLoggerModule> logger;

        public void Dispose()
        {
            //Nothing to dispose here
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            if (logger == null)
                logger = (AppConfig.GetService<ILoggerFactory>()
                            ?? throw new ArgumentNullException(nameof(ILoggerFactory)))
                            .CreateLogger<RequestLoggerModule>();

            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");
            var response = DiagnosticHelpers.GetProperty<HttpResponse>(context, "Response");

            logger.LogInformation($"End processing request, url '{request.Url}', response status '{response.Status}'");
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            if (logger == null)
                logger = (AppConfig.GetService<ILoggerFactory>()
                            ?? throw new ArgumentNullException(nameof(ILoggerFactory)))
                            .CreateLogger<RequestLoggerModule>();


            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");

            if (request == null || request.Url.ToString() == "/") return;

            logger.LogInformation($"Begin processing request, url '{request.Url}'");
        }
    }
}
