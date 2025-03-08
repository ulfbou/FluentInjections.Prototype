// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Metadata;

namespace FluentInjections.Orchestration
{
    public interface IModuleOrchestrator
    {
        Task ExecuteModulesAsync<TComponent>(IAsyncEnumerable<ModuleMetadata> moduleMetadata, CancellationToken? cancellationToken = null)
            where TComponent : IComponent;
    }
}
