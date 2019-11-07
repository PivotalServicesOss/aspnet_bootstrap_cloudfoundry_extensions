//using Microsoft.Extensions.Logging;
//using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
//using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
//using Steeltoe.Common.Diagnostics;
//using Steeltoe.Management.Census.Trace;
//using Steeltoe.Management.Census.Trace.Propagation;
//using Steeltoe.Management.Census.Trace.Unsafe;
//using Steeltoe.Management.Tracing;
//using System;
//using System.Collections.Concurrent;
//using System.Text.RegularExpressions;
//using System.Web;

//namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
//{
//    public class InboundRequestObserverModule : IHttpModule
//    {
//        internal ConcurrentDictionary<HttpRequest, ISpan> Pending = new ConcurrentDictionary<HttpRequest, ISpan>();

//        public void Dispose()
//        {
//            //Nothing to dispose here
//        }

//        public void Init(HttpApplication context)
//        {
//            context.BeginRequest += Context_BeginRequest;
//            context.EndRequest += Context_EndRequest;
//        }

//        private void Context_EndRequest(object sender, EventArgs e)
//        {
//            if (!AppBuilderExtensions.IncludeCorrelation)
//                return;

//            var context = ((HttpApplication)sender).Context;
//            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");
//            var response = DiagnosticHelpers.GetProperty<HttpResponse>(context, "Response");

//            HandleEndEvent(request, response);
//        }

//        private void Context_BeginRequest(object sender, EventArgs e)
//        {
//            if (!AppBuilderExtensions.IncludeCorrelation)
//                return;

//            var context = ((HttpApplication)sender).Context;
//            var request = DiagnosticHelpers.GetProperty<HttpRequest>(context, "Request");

//            if (request == null) return;

//            HandleBeginEvent(request);
//        }

//        private void HandleBeginEvent(HttpRequest request)
//        {
//            var tracing = DependencyContainer.GetService<ITracing>() ?? throw new ArgumentNullException(nameof(ITracing));
//            var tracingOptions = DependencyContainer.GetService<ITracingOptions>() ?? throw new ArgumentNullException(nameof(ITracingOptions));

//            if (ShouldIgnoreRequest(request.Url.AbsolutePath, tracingOptions))
//            {
//                this.Logger().LogDebug($"HandleBeginEvent: Ignoring path {request.Url.AbsolutePath}");
//                return;
//            }

//            if (Pending.TryGetValue(request, out ISpan span))
//            {
//                this.Logger().LogDebug($"HandleBeginEvent: Continuing existing span!");
//                return;
//            }

//            var spanName = GetSpanName(request);
//            var parentspanContext = GetParentSpanContext(request);

//            ISpan started;
//            if (parentspanContext != null)
//            {
//                started = tracing.Tracer.SpanBuilderWithRemoteParent(spanName, parentspanContext).StartSpan();
//            }
//            else
//            {
//                started = tracing.Tracer.SpanBuilder(spanName).StartSpan();
//            }

//            Pending.TryAdd(request, started);

//            started.PutClientSpanKindAttribute()
//                .PutHttpUrlAttribute(request.Url.ToString())
//                .PutHttpMethodAttribute(request.HttpMethod.ToString())
//                .PutHttpHostAttribute(request.Url.Host)
//                .PutHttpPathAttribute(request.Url.AbsolutePath)
//                .PutHttpRequestHeadersAttribute(request.Headers);

//            AsyncLocalContext.CurrentSpan = started;

//            InjectTraceContext(request, parentspanContext, tracing.Tracer, tracingOptions);
//        }

//        private void HandleEndEvent(HttpRequest request, HttpResponse response)
//        {
//            if (!Pending.TryRemove(request, out ISpan span))
//            {
//                this.Logger().LogDebug($"HandleEndEvent: Missing span context");
//                return;
//            }

//            if (span != null)
//            {
//                span.PutHttpStatusCodeAttribute(response.StatusCode);

//                if (response.Headers != null)
//                {
//                    span.PutHttpResponseHeadersAttribute(response.Headers);
//                }

//                span.End();

//                AsyncLocalContext.CurrentSpan = null;
//            }
//        }

//        private void InjectTraceContext(HttpRequest request, ISpanContext parentspanContext, ITracer tracer, ITracingOptions tracingOptions)
//        {
//            var traceId = tracer.CurrentSpan.Context.TraceId.ToLowerBase16();

//            if (traceId.Length > 16 && tracingOptions.UseShortTraceIds)
//            {
//                traceId = traceId.Substring(traceId.Length - 16, 16);
//            }

//            request.Headers.Add(B3Format.X_B3_TRACE_ID, traceId);
//            request.Headers.Add(B3Format.X_B3_SPAN_ID, tracer.CurrentSpan.Context.SpanId.ToLowerBase16());

//            if (tracer.CurrentSpan.Context.TraceOptions.IsSampled)
//            {
//                request.Headers.Add(B3Format.X_B3_SAMPLED, "1");
//            }

//            if (parentspanContext != null)
//            {
//                request.Headers.Add(B3Format.X_B3_PARENT_SPAN_ID, parentspanContext.SpanId.ToLowerBase16());
//            }
//        }

//        private static ISpanContext GetParentSpanContext(HttpRequest request)
//        {
//            if (!string.IsNullOrWhiteSpace(request.Headers[B3Format.X_B3_TRACE_ID])
//                    && !string.IsNullOrWhiteSpace(request.Headers[B3Format.X_B3_SPAN_ID]))
//            {
//                var traceId = request.Headers[B3Format.X_B3_TRACE_ID];
//                var spanId = request.Headers[B3Format.X_B3_SPAN_ID];

//                if (traceId.Length == 16)
//                    traceId = traceId + traceId;

//                return SpanContext.Create(TraceId.FromLowerBase16(traceId), SpanId.FromLowerBase16(spanId), TraceOptions.DEFAULT);
//            }
//            return null;
//        }

//        private string GetSpanName(HttpRequest request)
//        {
//            return $"httpclient:{request.Url.AbsolutePath}";
//        }

//        private bool ShouldIgnoreRequest(string absolutePath, ITracingOptions tracingOptions)
//        {
//            if (string.IsNullOrEmpty(absolutePath))
//                return false;

//            var pathMatcher = new Regex(tracingOptions.EgressIgnorePattern);

//            return pathMatcher.IsMatch(absolutePath);
//        }
//    }
//}
