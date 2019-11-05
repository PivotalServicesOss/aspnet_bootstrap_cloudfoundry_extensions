using Microsoft.Extensions.Logging;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base;
using PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Ioc;
using System;
using System.Collections.Generic;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Logging
{
    public static class GeneralExtensions
    {
        readonly static ILoggerFactory loggerFactory = DependencyContainer.GetService<ILoggerFactory>()
                                                        ?? throw new ArgumentNullException(nameof(ILoggerFactory));

        readonly static Dictionary<string, ILogger> loggers = new Dictionary<string, ILogger>();

        public static ILogger Logger(this object instance)
        {
            var callerName = instance.GetType().FullName;

            if (loggers.TryGetValue(callerName, out ILogger logger))
                return logger;

            return loggers[callerName] = loggerFactory.CreateLogger(callerName);
        }
    }
}
