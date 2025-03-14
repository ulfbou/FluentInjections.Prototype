// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.Internal.Discovery;
using FluentInjections.Metadata;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace FluentInjections.Internal.Discovery
{
    public interface IModuleDiscoveryService
    {
        IAsyncEnumerator<ModuleMetadata> DiscoverModulesAsync(IEnumerable<Assembly> assemblies, CancellationToken cancellationToken);
    }
}
