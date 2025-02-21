// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IMiddlewareBuilder<TBuilder, TContract> : IComponentBuilder<IMiddlewareComponent, TContract>
    where TBuilder : IApplicationBuilder<TBuilder>
{
    IApplication<TBuilder> Application { get; }
}
