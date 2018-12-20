using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pivotal.CloudFoundry.Replatform.Bootstrap.Base.Logging
{
    public class LoggerProvider
    {
        readonly static ILoggerFactory loggerFactory = AppConfig.GetService<ILoggerFactory>() 
                                                        ?? throw new ArgumentNullException(nameof(ILoggerFactory));
        readonly static Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();
        const string UNKNOWN_TYP_NM = "UnknownType";

        public static ILogger Logger
        {
            get
            {
                var callerName = new StackTrace().GetFrame(3).GetMethod().ReflectedType.FullName 
                                    ?? new StackTrace().GetFrame(2).GetMethod().ReflectedType.FullName
                                    ?? UNKNOWN_TYP_NM;

                if (loggers.TryGetValue(callerName, out ILogger logger))
                    return logger;

                return loggers[callerName] = loggerFactory.CreateLogger(callerName);
            }
        }
    }
}
