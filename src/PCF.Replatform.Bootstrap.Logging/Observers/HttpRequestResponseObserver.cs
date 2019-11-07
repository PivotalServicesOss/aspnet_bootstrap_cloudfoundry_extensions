using Microsoft.Extensions.Logging;
using OpenCensus.Common;
using OpenCensus.Trace;
using Steeltoe.Common.Diagnostics;
using Steeltoe.Management.Census.Trace;
using Steeltoe.Management.Tracing.Observer;
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging.Observers
{
    internal class HttpRequestResponseObserver : HttpClientTracingObserver
    {
        public const string START_EVNT = "Start";
        public const string STOP_EVNT = "Stop";
        public const string ERR_EVNT = "Error";

        private readonly ILogger logger;

        public HttpRequestResponseObserver(string observerName, string diagnosticName, ITracingOptions options, ITracing tracing, ILogger logger) 
            : base(observerName, diagnosticName, options, tracing, logger)
        {
            this.logger = logger;
        }

        public override void ProcessEvent(string evnt, object arg)
        {
            if (arg == null)
                return;

            if (evnt == START_EVNT)
            {
                logger.LogTrace($"HandleStartEvent start {Thread.CurrentThread.ManagedThreadId}");
                HandleStartEvent((HttpContext)arg);
                logger.LogTrace($"HandleStartEvent finished {Thread.CurrentThread.ManagedThreadId}");
            }
            else if (evnt == STOP_EVNT)
            {
                logger.LogTrace($"HandleStopEvent start {Thread.CurrentThread.ManagedThreadId}");
                HandleStopEvent((HttpContext)arg);
                logger.LogTrace($"HandleStopEvent finished {Thread.CurrentThread.ManagedThreadId}");
            }
            else if (evnt == ERR_EVNT)
            {
                logger.LogTrace($"HandleExceptionEvent start {Thread.CurrentThread.ManagedThreadId}");
                HandleExceptionEvent((HttpContext)arg);
                logger.LogTrace($"HandleExceptionEvent finished {Thread.CurrentThread.ManagedThreadId}");
            }
        }

        private void HandleExceptionEvent(HttpContext httpContext)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(httpContext, "Request");

            if (!request.RequestContext.RouteData.Values.TryGetValue("Steeltoe.SpanContext", out object value))
            {
                logger.LogDebug("HandleStopEvent: Missing span context");
                return;
            }

            var exception = httpContext.Error;

            ISpan active = (value as SpanContext).Active;

            if (active == null)
            {
                logger.LogDebug("HandleExceptionEvent: Active span missing, {exception}", exception);
            }
            else
            {
                Steeltoe.Management.Census.Trace.SpanExtensions.PutErrorStackTraceAttribute(
                    Steeltoe.Management.Census.Trace.SpanExtensions.PutErrorAttribute(active, 
                        GetExceptionMessage(exception)), GetExceptionStackTrace(exception)).Status = Status.Aborted;
            }
        }

        private void HandleStopEvent(HttpContext httpContext)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(httpContext, "Request");
            var response = DiagnosticHelpers.GetProperty<HttpResponse>(httpContext, "Response");

            if (!request.RequestContext.RouteData.Values.TryGetValue("Steeltoe.SpanContext", out object value))
            {
                logger.LogDebug("HandleStopEvent: Missing span context");
                return;
            }

            SpanContext spanContext = value as SpanContext;

            if (spanContext != null)
            {
                ISpan active = spanContext.Active;
                IScope activeScope = spanContext.ActiveScope;

                if (response != null)
                {
                    Steeltoe.Management.Census.Trace.SpanExtensions.PutHttpResponseHeadersAttribute(
                        OpenCensus.Trace.SpanExtensions.PutHttpStatusCodeAttribute(active, (int)response.StatusCode), response.Headers);
                }

                ((IDisposable)activeScope).Dispose();

                request.RequestContext.RouteData.Values.Remove("Steeltoe.SpanContext");
            }
        }

        private void HandleStartEvent(HttpContext httpContext)
        {
            var request = DiagnosticHelpers.GetProperty<HttpRequest>(httpContext, "Request");

            if (ShouldIgnoreRequest(request.Url.AbsolutePath))
            {
                logger.LogDebug($"HandleStartEvent: Ignoring path: {request.Url.AbsolutePath}");
                return;
            }

            if (request.RequestContext.RouteData.Values.TryGetValue("Steeltoe.SpanContext", out object _))
            {
                logger.LogDebug("HandleStartEvent: Continuing existing span!");
                return;
            }

            string text = ExtractSpanName(request);
            var currentSpan = GetCurrentSpan();
            var val = default(ISpan);

            var activeScope = (currentSpan == null) ? base.Tracer.SpanBuilder(text).StartScopedSpan(out val) 
                : base.Tracer.SpanBuilderWithExplicitParent(text, currentSpan).StartScopedSpan(out val);

            request.RequestContext.RouteData.Values.Add("Steeltoe.SpanContext", new SpanContext(val, activeScope));

            Steeltoe.Management.Census.Trace.SpanExtensions.PutHttpRequestHeadersAttribute(
                OpenCensus.Trace.SpanExtensions.PutHttpPathAttribute(
                     OpenCensus.Trace.SpanExtensions.PutHttpHostAttribute(
                         OpenCensus.Trace.SpanExtensions.PutHttpMethodAttribute(
                             OpenCensus.Trace.SpanExtensions.PutHttpRawUrlAttribute(
                                 OpenCensus.Trace.SpanExtensions.PutClientSpanKindAttribute(val), 
                                                    request.Url.ToString()), 
                                                request.HttpMethod.ToString()), 
                                        request.Url.Host, 
                                    request.Url.Port), 
                                request.Url.AbsolutePath), 
                             request.Headers);

            InjectTraceContext(request, currentSpan);
        }
        private string ExtractSpanName(HttpRequest request)
        {
            return $"httpclient:{request.Url.AbsolutePath}";
        }

        private void InjectTraceContext(HttpRequest request, ISpan parentSpan)
        {
            var headers = request.Headers;

            base.Propagation.Inject(base.Tracer.CurrentSpan.Context, headers, delegate (NameValueCollection collection, string key, string value)
            {
                if (key == "X-B3-TraceId" && value.Length > 16 && base.Options.UseShortTraceIds)
                {
                    value = value.Substring(value.Length - 16, 16);
                }
                collection.Add(key, value);
            });

            if (parentSpan != null)
            {
                headers.Add("X-B3-ParentSpanId", parentSpan.Context.SpanId.ToLowerBase16());
            }
        }
    }
}
