// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections;

internal abstract class ComponentRegistration<TComponent> : IComponentRegistration<TComponent> where TComponent : IComponent
{
    protected IComponentDescriptor<TComponent>? _descriptor;

    public required Type ContractType { get; set; }
    public required string Alias { get; set; }

    public IComponentDescriptor<TComponent> ComponentDescriptor => CreateDescriptor();

    protected abstract IComponentDescriptor<TComponent> CreateDescriptor();
}
