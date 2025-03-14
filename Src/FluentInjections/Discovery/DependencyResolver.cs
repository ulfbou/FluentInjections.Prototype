// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Discovery;
using FluentInjections.Exceptions;
using FluentInjections.Validation;

using Microsoft.Extensions.Logging;

namespace FluentInjections.DependencyResolution
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly ILogger<DependencyResolver> _logger;

        public DependencyResolver(ILogger<DependencyResolver> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IExecutionGraph<TItem>> BuildExecutionGraphAsync<TItem>(
            IEnumerable<DiscoveryMetadata<TItem>> items,
            CancellationToken cancellationToken = default)
        {
            Guard.NotNull(items, nameof(items));

            try
            {
                var itemDict = items.ToDictionary(x => x.Name, x => x);
                var inDegree = itemDict.Keys.ToDictionary(k => k, k => 0);
                var adjList = itemDict.Keys.ToDictionary(k => k, k => new HashSet<string>());

                foreach (var item in itemDict.Values)
                {
                    foreach (var dep in item.Dependencies)
                    {
                        if (itemDict.ContainsKey(dep))
                        {
                            inDegree[item.Name]++;
                            adjList[dep].Add(item.Name);
                        }
                        else
                        {
                            _logger.LogWarning("Dependency '{DependencyName}' not found for item '{ItemName}'.", dep, item.Name);
                        }
                    }
                }

                var queue = new Queue<string>(inDegree.Where(kv => kv.Value == 0).Select(kv => kv.Key));
                var sortedList = new List<DiscoveryMetadata<TItem>>();

                while (queue.Count > 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var name = queue.Dequeue();
                    sortedList.Add(itemDict[name]);

                    foreach (var dependent in adjList[name])
                    {
                        inDegree[dependent]--;
                        if (inDegree[dependent] == 0)
                            queue.Enqueue(dependent);
                    }
                }

                if (sortedList.Count != itemDict.Count)
                {
                    var missingItems = itemDict.Keys.Except(sortedList.Select(x => x.Name)).ToList();
                    if (missingItems.Any())
                    {
                        throw new MissingDependencyException($"Missing dependencies: {string.Join(", ", missingItems)}");
                    }
                    else
                    {
                        throw new CircularDependencyException("Circular dependency detected.");
                    }
                }

                return Task.FromResult<IExecutionGraph<TItem>>(new ExecutionGraph<TItem>(sortedList));
            }
            catch (MissingDependencyException)
            {
                throw;
            }
            catch (CircularDependencyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building execution graph.");
                throw new DependencyResolutionException("An error occurred while building the execution graph.", ex);
            }
        }
    }
}
