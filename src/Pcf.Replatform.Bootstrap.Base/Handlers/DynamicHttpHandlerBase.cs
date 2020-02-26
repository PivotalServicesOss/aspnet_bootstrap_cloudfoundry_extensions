using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System;
using System.Threading.Tasks;
using System.Web;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public abstract class DynamicHttpHandlerBase : IDynamicHttpHandler
    {
        protected ILogger<DynamicHttpHandlerBase> logger;
        protected EventHandlerTaskAsyncHelper asyncHandlerHelper;

        public DynamicHttpHandlerBase(ILogger<DynamicHttpHandlerBase> logger = null)
        {
            this.logger = logger ?? DependencyContainer.GetService<ILogger<DynamicHttpHandlerBase>>(true);
            asyncHandlerHelper = new EventHandlerTaskAsyncHelper(HandleAsyncRequest);
        }

        public abstract string Path { get; }
        public abstract DynamicHttpHandlerEvent ApplicationEvent { get; }
        public abstract void HandleRequest(HttpContextBase context);

        public void RegisterEvent(HttpApplication application)
        {
            switch (ApplicationEvent)
            {
                case DynamicHttpHandlerEvent.AuthenticateRequestAsync:
                    application.AddOnAuthenticateRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.AuthorizeRequestAsync:
                    application.AddOnAuthorizeRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.BeginRequestAsync:
                    application.AddOnBeginRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.EndRequestAsync:
                    application.AddOnEndRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.LogRequestAsync:
                    application.AddOnLogRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.PostAuthenticateRequestAsync:
                    application.AddOnPostAuthenticateRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.PostAuthorizeRequestAsync:
                    application.AddOnPostAuthorizeRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.PostLogRequestAsync:
                    application.AddOnPostLogRequestAsync(asyncHandlerHelper.BeginEventHandler, asyncHandlerHelper.EndEventHandler);
                    break;
                case DynamicHttpHandlerEvent.BeginRequest:
                    application.BeginRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.AuthenticateRequest:
                    application.AuthenticateRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.PostAuthenticateRequest:
                    application.PostAuthenticateRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.AuthorizeRequest:
                    application.AuthorizeRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.PostAuthorizeRequest:
                    application.PostAuthorizeRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.PostLogRequest:
                    application.PostLogRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.LogRequest:
                    application.LogRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.EndRequest:
                    application.EndRequest += HandleRequest;
                    break;
                case DynamicHttpHandlerEvent.Error:
                    application.Error += HandleRequest;
                    break;
                default:
                    throw new ApplicationException($"Async event type '{ApplicationEvent}' not configured for registrations");
            }
        }

        /// <summary>
        /// Default is true, but access can be restricted based on permission here
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool IsEnabled(HttpContextBase context)
        {
            return true;
        }

        /// <summary>
        /// Should continue processing the request after this handler, default is false
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool ContinueNext(HttpContextBase context)
        {
            return false;
        }

        internal bool IsPathMatched(HttpContextBase context)
        {
            if (!string.IsNullOrWhiteSpace(Path) && context.Request.Path.Contains(Path))
                return true;
            else if (string.IsNullOrWhiteSpace(Path))
                return true;

            return false;
        }

        private void HandleRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            FilterAndProcessRequest(context, context.ApplicationInstance.CompleteRequest);
        }

        private void FilterAndProcessRequest(HttpContextBase context, Action completeRequest)
        {
            if (IsPathMatched(context))
            {
                if (IsEnabled(context))
                    HandleRequest(context);

                if (!ContinueNext(context))
                    completeRequest();
            }
        }

        private async Task HandleAsyncRequest(object sender, EventArgs e)
        {
            var context = new HttpContextWrapper(((HttpApplication)sender).Context);
            await FilterAndProcessRequestAsync(context, context.ApplicationInstance.CompleteRequest)
                                            .ConfigureAwait(continueOnCapturedContext: false);
        }

        private async Task FilterAndProcessRequestAsync(HttpContextBase context, Action completeRequest)
        {
            if (IsPathMatched(context))
            {
                if (await Task.Run(() => IsEnabled(context)))
                    HandleRequest(context);

                if (!await Task.Run(() => ContinueNext(context)))
                    completeRequest();
            }
        }

        protected internal string GetRequestUri(HttpRequestBase request)
        {
            string str = request.IsSecureConnection ? "https" : "http";
            string text = request.Headers.Get("X-Forwarded-Proto");
            if (text != null)
                str = text;

            return str + "://" + request.Url.Host + request.Path.ToString();
        }
    }
}