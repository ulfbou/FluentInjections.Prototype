﻿// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components;

public interface IComponentCreationDescriptor<out TComponent, TContract>
where TComponent : IComponent
{
    TContract? Instance { get; init; }
    Func<IServiceProvider, CancellationToken?, ValueTask<TContract>>? Factory { get; init; }
    Action<TContract>? Configure { get; init; }
    Func<IServiceProvider, TContract>? Decorator { get; init; }
}
