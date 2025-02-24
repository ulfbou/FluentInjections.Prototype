// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

namespace FluentInjections.Middlewares;

internal class MiddlewareRegistration<TContract>
    : ComponentRegistration<IMiddlewareComponent, TContract>
    , IComponentRegistration<IMiddlewareComponent, TContract>
{ }
