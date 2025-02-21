// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

public interface IComponentDescriptor<out TComponent>
{
    public string Alias { get; }
    ComponentLifetime Lifetime { get; }
    Type ContractType { get; }
}
public interface IComponentDescriptor<out TComponent, TContract> : IComponentDescriptor<TComponent>
{
    public Func<IServiceProvider, TContract>? Factory { get; }
    public TContract? Instance { get; }
    public Action<TContract>? Configure { get; }
    public IDictionary<string, object?> Dependencies { get; }
    public Func<IServiceProvider, TContract>? Decorator { get; }


}
