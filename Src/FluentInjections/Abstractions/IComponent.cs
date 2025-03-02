// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// A marker interface that represents a component.
    /// </summary>
    public interface IComponent { }

    /// <summary>
    /// A marker interface that represents a service component.
    /// </summary>
    public interface IServiceComponent : IComponent { }

    /// <summary>
    /// A marker interface that represents a middleware component.
    /// </summary>
    public interface IMiddlewareComponent : IComponent { }

    /// <summary>
    /// A marker interface that represents a lifecycle component.
    /// </summary>
    public interface ILifecycleComponent : IComponent { }
}
