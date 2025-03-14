// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Utilities.Collections
{
    public sealed class CompositeDictionary<TKey, TValue> where TKey : notnull
    {
        private readonly List<IDictionary<TKey, TValue>> _sources = new();

        public void AddSource(IDictionary<TKey, TValue> source) => _sources.Add(source);

        public bool TryGetValue(TKey key, out TValue? value)
        {
            foreach (var source in _sources)
            {
                if (source.TryGetValue(key, out value))
                    return true;
            }

            value = default;
            return false;
        }
    }
}
