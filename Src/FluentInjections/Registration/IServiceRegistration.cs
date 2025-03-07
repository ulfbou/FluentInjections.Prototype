// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.Registration
{
    public interface ILifecycleRegistration<TContract> : IComponentRegistration<ILifecycleComponent, TContract> { }
    public interface IMiddlewareRegistration<TContract> : IComponentRegistration<IMiddlewareComponent, TContract> { }
    public interface IServiceRegistration<TContract> : IComponentRegistration<IServiceComponent, TContract> { }
}
