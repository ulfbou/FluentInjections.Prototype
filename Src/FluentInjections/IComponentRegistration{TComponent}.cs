// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.Contracts;

namespace FluentInjections;

public interface IComponentRegistration<out TComponent>
{
    Type ContractType { get; set; }
    string Alias { get; set; }


    IComponentDescriptor<TComponent> ComponentDescriptor { get; }
}
