// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IComponentRegistration<out TComponent, TContract> : IComponentRegistration<TComponent>
    where TComponent : IComponent
{
    Type? ResolutionType { get; set; }
    Func<IServiceProvider, TContract>? Factory { get; set; }
    TContract? Instance { get; set; }
    ComponentLifetime Lifetime { get; set; }
    Action<TContract>? Configure { get; set; }
    Func<IServiceProvider, bool>? Condition { get; set; }
    IDictionary<string, object?> Metadata { get; }
    object? Parameters { get; set; }
    IDictionary<string, object?> Dependencies { get; set; }
    Func<IServiceProvider, TContract>? Decorator { get; set; }
}
