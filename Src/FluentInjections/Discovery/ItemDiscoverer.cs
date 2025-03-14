// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace FluentInjections.Discovery
{
    public class ItemDiscoverer : IItemDiscoverer
    {
        private readonly MemoryCache _cache;
        private readonly ILogger<ItemDiscoverer> _logger;
        private readonly ItemDiscovererOptions _options;

        public ItemDiscoverer(IMemoryCache cache, ILogger<ItemDiscoverer> logger, IOptions<ItemDiscovererOptions> options)
        {
            _cache = cache as MemoryCache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public Task<IEnumerable<DiscoveryMetadata<TItem>>> DiscoverItemsAsync<TItem>(
            Assembly assembly,
            Func<Type, bool> predicate = null!,
            CancellationToken cancellationToken = default)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            var cacheKey = $"{_options.CacheKeyPrefix}{typeof(TItem).FullName}_{assembly.FullName}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<DiscoveryMetadata<TItem>>? cachedItems))
            {
                _logger.LogDebug("Retrieved discovered items from cache for {CacheKey}", cacheKey);
                return Task.FromResult(cachedItems)!;
            }

            var result = new List<DiscoveryMetadata<TItem>>();
            var types = Array.Empty<Type>();

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                _logger.LogError(ex, "Error loading types from assembly {Assembly}. Loaded types: {LoadedTypes}, Loader Exceptions: {LoaderExceptions}", assembly.FullName, ex.Types, ex.LoaderExceptions);
                types = (ex.Types.Where(t => t != null).ToArray() ?? Array.Empty<Type>())!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting types from assembly {Assembly}", assembly.FullName);
                throw;
            }

            try
            {
                foreach (var type in types)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (type == null || !type.IsClass || type.IsAbstract)
                        continue;

                    if (!typeof(TItem).IsAssignableFrom(type))
                        continue;

                    if (predicate != null && !predicate(type))
                        continue;

                    var metadata = new DiscoveryMetadata<TItem>
                    {
                        Name = type.Name,
                        Version = assembly.GetName().Version?.ToString() ?? "1.0.0",
                        Priority = 0,
                        ItemType = type
                    };

                    var dependsOnAttributes = type.GetCustomAttributes(typeof(DependsOnAttribute), false) as DependsOnAttribute[];
                    if (dependsOnAttributes != null)
                    {
                        foreach (var attr in dependsOnAttributes)
                        {
                            metadata.Dependencies.Add(attr.DependencyName);
                        }
                    }

                    result.Add(metadata);
                }

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(_options.CacheExpirationMinutes));
                _logger.LogDebug("Discovered and cached items for {CacheKey}", cacheKey);
                return Task.FromResult(result.AsEnumerable());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error discovering items in assembly {Assembly}", assembly.FullName);
                throw;
            }
        }
    }
}
