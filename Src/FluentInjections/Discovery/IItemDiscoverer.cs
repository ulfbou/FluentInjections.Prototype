// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

namespace FluentInjections.Discovery
{
    public interface IItemDiscoverer
    {
        Task<IEnumerable<DiscoveryMetadata<TItem>>> DiscoverItemsAsync<TItem>(
            Assembly assembly,
            Func<Type, bool> predicate = null!,
            CancellationToken cancellationToken = default);
    }
}
