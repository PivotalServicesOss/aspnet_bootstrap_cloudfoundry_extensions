using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Management.Census.Trace;
using System;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
{
    public class InboundRequestObserverModule : IHttpModule
    {
        HttpRequestResponseObserver observer;

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
            context.Error += Context_Error;
        }

        private void Context_Error(object sender, EventArgs e)
        {
            if (!AppBuilderExtensions.IncludeDistributedTracing)
                return;

            InitializeObserver();

            observer.ProcessEvent(HttpRequestResponseObserver.ERR_EVNT, ((HttpApplication)sender).Context);
        }

        
        private void Context_EndRequest(object sender, EventArgs e)
        {
            if (!AppBuilderExtensions.IncludeDistributedTracing)
                return;

            InitializeObserver();

            observer.ProcessEvent(HttpRequestResponseObserver.STOP_EVNT, ((HttpApplication)sender).Context);
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            if (!AppBuilderExtensions.IncludeDistributedTracing)
                return;

            InitializeObserver();

            var context = ((HttpApplication)sender).Context;
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");

            if (request == null) return;

            observer.ProcessEvent(HttpRequestResponseObserver.START_EVNT, ((HttpApplication)sender).Context);
        }

        public void Dispose()
        {

        }

        private void InitializeObserver()
        {
            if (observer == null)
                observer = new HttpRequestResponseObserver("HttpRequestResponseObserver",
                                        "RequestResponseDiagnostics",
                                        DependencyContainer.GetService<ITracingOptions>() ?? throw new ArgumentNullException(nameof(ITracingOptions)),
                                        DependencyContainer.GetService<ITracing>() ?? throw new ArgumentNullException(nameof(ITracing)),
                                        this.Logger());
        }

    }
}
