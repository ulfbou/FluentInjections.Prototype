// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components;

public interface IComponentRegistration<out TComponent, TContract> : IComponentRegistration<TComponent>
where TComponent : IComponent
{
    Type? ResolutionType { get; set; }
    Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Factory { get; set; }
    TContract? Instance { get; set; }
    ComponentLifetime Lifetime { get; set; }
    Action<TContract>? Configure { get; set; }
    Func<IServiceProvider, bool>? Condition { get; set; }
    IDictionary<string, object?> Metadata { get; }
    IDictionary<string, object?> Parameters { get; set; }
    IDictionary<string, Type> Dependencies { get; set; }
    Func<IServiceProvider, CancellationToken, ValueTask<TContract>>? Decorator { get; set; }
}
