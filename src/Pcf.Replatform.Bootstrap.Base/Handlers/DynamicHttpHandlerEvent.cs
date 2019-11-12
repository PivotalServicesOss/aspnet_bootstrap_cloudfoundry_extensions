namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base.Handlers
{
    public enum DynamicHttpHandlerEvent
    {
        AuthenticateRequestAsync,
        AuthorizeRequestAsync,
        BeginRequestAsync,
        EndRequestAsync,
        LogRequestAsync,
        PostAuthenticateRequestAsync,
        PostAuthorizeRequestAsync,
        PostLogRequestAsync,
        BeginRequest,
        AuthenticateRequest,
        PostAuthenticateRequest,
        AuthorizeRequest,
        PostAuthorizeRequest,
        PostLogRequest,
        LogRequest,
        EndRequest,
        Error
    }
}
