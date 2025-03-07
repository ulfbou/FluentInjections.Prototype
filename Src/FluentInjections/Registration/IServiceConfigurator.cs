// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.Registration
{
    public interface IServiceConfigurator : IConfigurator<IServiceComponent> { }
    public interface ILifecycleConfigurator : IConfigurator<ILifecycleComponent> { }
    public interface IMiddlewareConfigurator : IConfigurator<IMiddlewareComponent> { }
}
