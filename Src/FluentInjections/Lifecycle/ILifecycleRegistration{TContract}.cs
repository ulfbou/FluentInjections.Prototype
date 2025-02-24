// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

namespace FluentInjections.Lifecycle
{
    internal interface ILifecycleRegistration<TContract> : IComponentRegistration<ILifecycleComponent, TContract> { }
}
