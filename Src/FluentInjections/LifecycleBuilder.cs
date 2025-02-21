// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


using Microsoft.Extensions.Logging;

namespace FluentInjections;

internal class LifecycleBuilder<TContract, TRegistration>
    : ComponentBuilderBase<ILifecycleComponent, TContract, TRegistration>
    , IComponentBuilder<ILifecycleComponent, TContract>
        where TRegistration : IComponentRegistration<ILifecycleComponent, TContract>
{
    public LifecycleBuilder(TRegistration registration, ILoggerFactory loggerFactory) : base(registration, loggerFactory) { }
}
