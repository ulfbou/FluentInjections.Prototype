// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class LifecycleComponentBuilder<TComponent, TContract, TRegistration>
    : ComponentBuilderBase<TComponent, TContract, TRegistration>
    , ILifecycleBuilder, IComponentBuilder<TComponent, TContract>
        where TComponent : IComponent
        where TRegistration : IComponentRegistration<TComponent, TContract>
{
    public LifecycleComponentBuilder(TRegistration registration, ILoggerFactory loggerFactory)
        : base(registration, loggerFactory)
    { }
}
