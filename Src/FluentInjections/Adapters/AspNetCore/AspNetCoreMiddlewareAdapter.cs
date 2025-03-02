// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace FluentInjections.Adapters.AspNetCore
{
    //internal class AspNetCoreMiddlewareAdapter
    //{
    //    private readonly Func<ApplicationDelegate, ApplicationDelegate> _abstractMiddleware;

    //    public AspNetCoreMiddlewareAdapter(Func<ApplicationDelegate, ApplicationDelegate> abstractMiddleware)
    //    {
    //        _abstractMiddleware = abstractMiddleware ?? throw new ArgumentNullException(nameof(_abstractMiddleware));
    //    }

    //    public Func<RequestDelegate, RequestDelegate> ToAspNetCoreMiddleware()
    //    {
    //        return nextRequestDelegate =>
    //        {
    //            return async httpContext =>
    //            {
    //                var appContext = new AspNetCoreApplicationContext(httpContext);
    //                ApplicationDelegate adaptedNextDelegate = abstractAppContext =>
    //                {
    //                    return nextRequestDelegate(((AspNetCoreApplicationContext)abstractAppContext).HttpContext);
    //                };
    //                await _abstractMiddleware(adaptedNextDelegate)(appContext);
    //            };
    //        };
    //    }
    //}
}
