using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Hosting;

namespace Pcf.Replatform.Bootstrap.Diagnostics
{
    public class GlobalErrorHandlerModule : IHttpModule
    {
        ILogger<GlobalErrorHandlerModule> logger;
        public GlobalErrorHandlerModule()
        {
            logger = (CoreServiceConfig.GetService<ILoggerFactory>() 
                        ?? throw new ArgumentNullException(nameof(ILoggerFactory)))
                        .CreateLogger<GlobalErrorHandlerModule>();
        }

        public void Dispose()
        {
            //Nothing to dispose here
        }

        public void Init(HttpApplication context)
        {
            context.Error += Context_Error;
        }

        private void Context_Error(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            try
            {
                var lastError = context.Server.GetLastError();

                if (lastError == null)
                    lastError = new Exception("Unknown/Unhandled application error, no further details available");

                LogError(lastError);
            }
            catch(Exception exception)
            {
                LogError(exception);
            }
        }

        private void LogError(Exception exception)
        {
            logger.Log(LogLevel.Error, exception, exception.ToString());

            EventLog.WriteEntry(HostingEnvironment.ApplicationHost.GetSiteName(), exception.ToString());
        }
    }
}
