// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.ObjectModel;

using FluentInjections.Discovery;
using FluentInjections.Validation;

namespace FluentInjections.DependencyResolution
{
    public class ExecutionGraph<TItem> : IExecutionGraph<TItem>
    {
        private readonly ReadOnlyCollection<DiscoveryMetadata<TItem>> _sortedItems;

        public ExecutionGraph(IEnumerable<DiscoveryMetadata<TItem>> sortedItems)
        {
            Guard.NotNull(sortedItems, nameof(sortedItems));

            if (sortedItems.Any(item => item == null))
            {
                throw new ArgumentException("The input list contains null items.", nameof(sortedItems));
            }

            _sortedItems = sortedItems.ToList().AsReadOnly();
        }

        public IReadOnlyList<DiscoveryMetadata<TItem>> SortedItems => _sortedItems;
    }
}