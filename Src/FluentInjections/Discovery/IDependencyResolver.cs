// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Discovery;

namespace FluentInjections.DependencyResolution
{
    public interface IDependencyResolver
    {
        Task<IExecutionGraph<TItem>> BuildExecutionGraphAsync<TItem>(
            IEnumerable<DiscoveryMetadata<TItem>> items,
            CancellationToken cancellationToken = default);
    }
}
