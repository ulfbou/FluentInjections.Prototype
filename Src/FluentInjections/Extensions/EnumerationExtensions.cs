// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Extensions
{
    public static class EnumerationExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> AsEnumerable<T>(this T item, params T[] items)
        {
            yield return item;
            foreach (var i in items)
            {
                yield return i;
            }
        }

        // Filter out null values from an enumeration
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> items)
        {
            return items.Where(i => i is not null);
        }
    }
}
