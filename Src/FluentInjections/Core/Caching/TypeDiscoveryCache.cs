// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Concurrent;

namespace FluentInjections.Core.Caching
{
    public class TypeDiscoveryCache
    {
        private readonly ConcurrentDictionary<CacheKey, Lazy<Task<List<System.Type>>>> _cache = new();

        public async Task<List<System.Type>> GetOrAddAsync(CacheKey key, Func<CacheKey, Task<List<System.Type>>> valueFactory)
        {
            var lazyResult = _cache.GetOrAdd(key, new Lazy<Task<List<System.Type>>>(() => valueFactory(key)));
            return await lazyResult.Value;
        }
    }
}
