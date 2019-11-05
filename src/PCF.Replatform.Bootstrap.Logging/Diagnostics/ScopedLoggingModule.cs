using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using Serilog.Context;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
{
    public class ScopedLoggingModule : IHttpModule
    {
        IEnumerable<IDynamicMessageProcessor> messageProcessors;
        const string CORR_CONTXT = "CorrelationContext";
        const string REQ_PATH_LOG_PROP_NM = "RequestPath";

        public void Dispose()
        {
            //Nothing to dispose here
        }

        public void Init(HttpApplication context)
        {
            if (!AppBuilderExtensions.IncludeCorrelation)
                return;

            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            if (messageProcessors == null)
            {
                messageProcessors = DependencyContainer.GetService<IEnumerable<IDynamicMessageProcessor>>()
                   ?? throw new ArgumentNullException(nameof(IEnumerable<IDynamicMessageProcessor>));

                if (!messageProcessors.Any())
                    throw new Exception("No message procesors of type 'IDynamicMessageProcessor' found");
            }

            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");

            if (request == null) return;

            PushCorelationProperties(request);
        }

        private void PushCorelationProperties(HttpRequest request)
        {
            var correlationContextInfo = string.Empty;

            foreach (var processor in messageProcessors)
            {
                correlationContextInfo = processor.Process(correlationContextInfo);
            }

            LogContext.PushProperty(CORR_CONTXT, correlationContextInfo, true);
            LogContext.PushProperty(REQ_PATH_LOG_PROP_NM, request.Url, true);
        }
    }
}
