﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Actuators.Testing
{
    [Obsolete("Not for production use, only for internal testing purpose")]
    public class TestProxy
    {
        public static void AddControllersProxy(IServiceCollection services)
        {
            services.AddControllers();
        }
    }
}