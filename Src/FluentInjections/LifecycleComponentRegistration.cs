// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

internal class LifecycleRegistration<TContract>
    : ComponentRegistration<ILifecycleComponent, TContract>
    , IComponentRegistration<ILifecycleComponent, TContract>
{ }
