// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

using Microsoft.AspNetCore.Http;

using Moq;

namespace FluentInjections.Middlewares;

internal class MiddlewareRegistration<TContract>
    : ComponentRegistration<IMiddlewareComponent, TContract>
    , IMiddlewareRegistration<TContract>
    , IComponentRegistration<IMiddlewareComponent, TContract>
{
    public const int DefaultPriority = 0;
    public const string DefaultGroup = nameof(DefaultGroup);

    public int Priority { get; set; } = DefaultPriority;
    public Func<HttpContext, Task>? Fallback { get; set; }
    public List<Type> PrecedingMiddleware { get; set; } = new();
    public List<Type> FollowingMiddleware { get; set; } = new();
}
