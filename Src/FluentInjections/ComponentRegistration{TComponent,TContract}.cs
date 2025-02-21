// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Descriptors;
using FluentInjections.Validation;

namespace FluentInjections;

public class ComponentRegistration<TComponent, TContract>
    : ComponentRegistration<TComponent>
    , IComponentRegistration<TComponent, TContract> where TComponent : IComponent
{
    public Func<IServiceProvider, TContract>? Factory { get; set; }
    public TContract? Instance { get; set; }
    public ComponentLifetime Lifetime { get; set; }
    public Action<TContract>? Configure { get; set; }
    public Func<IServiceProvider, bool>? Condition { get; set; }
    public IDictionary<string, object?> Metadata { get; } = new Dictionary<string, object?>();
    public object? Parameters { get; set; }
    public IDictionary<string, object?> Dependencies { get; set; } = new Dictionary<string, object?>();
    public Func<IServiceProvider, TContract>? Decorator { get; set; }
    public Type? ResolutionType { get; set; }

    protected override IComponentDescriptor<TComponent> CreateDescriptor()
    {
        return new ComponentDescriptor<TComponent, TContract>(this);
    }
}
