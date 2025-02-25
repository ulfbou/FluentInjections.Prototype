// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Application;
using FluentInjections.Components;

namespace FluentInjections.Middlewares
{
    public interface IMiddlewareBuilder<out TBuilder, TContract> : IComponentBuilder<IMiddlewareComponent, TContract>
        where TBuilder : IApplicationBuilder<TBuilder>
    {
        IApplication<TBuilder> Application { get; }
    }
}
