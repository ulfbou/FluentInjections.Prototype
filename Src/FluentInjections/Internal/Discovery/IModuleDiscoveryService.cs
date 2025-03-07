// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Internal.Discovery;
using FluentInjections.Metadata;

using System.Reflection;

namespace FluentInjections.Internal.Discovery
{
    public interface IModuleDiscoveryService
    {
        IAsyncEnumerable<ModuleMetadata> DiscoverModulesAsync(IEnumerable<Assembly> assemblies = null!, CancellationToken cancellationToken = default);
    }
}
