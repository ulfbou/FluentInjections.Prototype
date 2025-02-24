// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components;

public interface IComponentDescriptor<out TComponent, TContract> : IComponentDescriptor<TComponent>
    where TComponent : IComponent
{
    public Func<IServiceProvider, CancellationToken?, ValueTask<TContract>>? Factory { get; }
    public TContract? Instance { get; }
    public Action<TContract>? Configure { get; }
    public IDictionary<string, object?> Dependencies { get; }
    public Func<IServiceProvider, TContract>? Decorator { get; }
}
