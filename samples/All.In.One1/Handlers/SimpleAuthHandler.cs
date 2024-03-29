﻿using PivotalServices.AspNet.Bootstrap.Extensions.Handlers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace All.In.One.Handlers
{
    public class SimpleAuthHandler : DynamicHttpHandlerBase
    {
        public override string Path => null;

        public override DynamicHttpHandlerEvent ApplicationEvent => DynamicHttpHandlerEvent.AuthenticateRequestAsync;

        public override void HandleRequest(HttpContextBase context)
        {
            var nameClaim = new Claim(ClaimTypes.Name, "FooUser");
            var identity = new ClaimsIdentity(new[] { nameClaim });
            context.User = new ClaimsPrincipal(identity);
        }

        public override bool ContinueNext(HttpContextBase context)
        {
            return true;
        }
    }
}