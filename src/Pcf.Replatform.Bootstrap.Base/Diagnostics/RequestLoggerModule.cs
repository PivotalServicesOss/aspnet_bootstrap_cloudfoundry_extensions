using Microsoft.Extensions.Logging;
using Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Extensions;
using Steeltoe.Common.Diagnostics;
using System;
using System.Web;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Diagnostics
{
    public class RequestLoggerModule : IHttpModule
    {
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
            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");
            var response = DiagnosticHelpers.GetProperty<HttpResponse>(context, "Response");

            this.Logger().LogDebug($"End processing request, url '{request.Url}', response status '{response.Status}'");
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");

            if (request == null || request.Url.ToString() == "/") return;

            this.Logger().LogDebug($"Begin processing request, url '{request.Url}'");
        }
    }
}
