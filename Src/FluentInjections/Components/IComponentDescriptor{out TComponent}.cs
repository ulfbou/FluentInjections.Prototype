// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components;

public interface IComponentDescriptor<out TComponent> where TComponent : IComponent
{
    public string Alias { get; }
    ComponentLifetime Lifetime { get; }
    Type ContractType { get; }
}
