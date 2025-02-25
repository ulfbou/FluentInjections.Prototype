// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

using Microsoft.AspNetCore.Http;

namespace FluentInjections.Middlewares
{
    internal interface IMiddlewareRegistration<TContract> : IComponentRegistration<IMiddlewareComponent, TContract>
    {
        int Priority { get; set; }
        Func<HttpContext, Task>? Fallback { get; set; }
        List<Type> PrecedingMiddleware { get; set; }
        List<Type> FollowingMiddleware { get; set; }
    }
}
