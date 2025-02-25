// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Components
{
    public interface IComponentRegistration<out TComponent>
    {
        Type ContractType { get; }
        string Alias { get; }
    }
}
