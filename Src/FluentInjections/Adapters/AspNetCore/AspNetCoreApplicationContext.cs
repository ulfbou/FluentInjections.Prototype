// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace FluentInjections.Adapters.AspNetCore
{
    // Concrete Implementation for ASP.NET Core Context (AspNetCoreApplicationContext.cs)
    //public class AspNetCoreApplicationContext : IApplicationContext
    //{
    //    private readonly HttpContext _httpContext;

    //    public AspNetCoreApplicationContext(HttpContext httpContext)
    //    {
    //        _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
    //    }

    //    public HttpContext HttpContext => _httpContext;

    //    public CancellationToken CancellationToken => _httpContext.RequestAborted;
    //}
}
