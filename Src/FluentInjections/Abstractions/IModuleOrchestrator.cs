// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    public interface IModuleOrchestrator
    {
        ValueTask ExecuteModulesAsync<TComponent>() where TComponent : IComponent;
    }
}
