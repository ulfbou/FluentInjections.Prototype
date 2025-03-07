// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.Registration
{
    public interface IServiceComponentBuilder<TContract> : IComponentBuilder<IServiceComponent, TContract> { }
    public interface ILifecycleComponentBuilder<TContract> : IComponentBuilder<ILifecycleComponent, TContract> { }
    public interface IMiddlewareComponentBuilder<TContract> : IComponentBuilder<IMiddlewareComponent, TContract> { }
}
