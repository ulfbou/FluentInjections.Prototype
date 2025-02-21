// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

internal class MiddlewareRegistration<TContract>
    : ComponentRegistration<IMiddlewareComponent, TContract>
    , IComponentRegistration<IMiddlewareComponent, TContract>
{ }
