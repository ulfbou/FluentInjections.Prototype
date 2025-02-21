using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FluentInjections.Extensions;

public static class AsyncEnumerableExtensions
{
    public static async Task<IEnumerable<T>> ToEnumerableAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        var list = new List<T>();
        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            list.Add(item);
        }
        return list;
    }

    public static IAsyncEnumerable<T> ToEnumerableAsync<T>(this IEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        return ToAsyncInternal(source, cancellationToken);

        static async IAsyncEnumerable<T> ToAsyncInternal(
            IEnumerable<T> source,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var item in source)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return item;
                await Task.Yield();
            }
        }
    }

    public static async Task<T[]> ToArrayAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        var list = await source.ToEnumerableAsync(cancellationToken).ConfigureAwait(false);
        return list.ToArray();
    }

    public static IAsyncEnumerable<T> ToEnumerableAsync<T>(this T[] source, CancellationToken cancellationToken = default)
    {
        return ToAsyncInternal(source, cancellationToken);
        static async IAsyncEnumerable<T> ToAsyncInternal(
            T[] source,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var item in source)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return item;
                await Task.Yield();
            }
        }
    }

    public static async Task<TDictionary> ToDictionaryAsync<TDictionary, TKey, TValue>(
        this IAsyncEnumerable<TValue> source,
        Func<TValue, TKey> keySelector,
        CancellationToken cancellationToken = default)
        where TDictionary : IDictionary<TKey, TValue>, new()
        where TKey : notnull
    {
        var dictionary = new TDictionary();

        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            dictionary.TryAdd(keySelector(item), item);
        }

        return dictionary;
    }

    public static async Task<ICollection<T>> ToCollectionAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        ICollection<T> collection = new List<T>();

        await foreach (var item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            collection.Add(item);
        }

        return collection;
    }

    public static async Task<IReadOnlyCollection<T>> ToReadOnlyAsync<T>(this IAsyncEnumerable<T> source, CancellationToken cancellationToken = default)
    {
        var collection = await source.ToCollectionAsync(cancellationToken).ConfigureAwait(false);
        return (IReadOnlyCollection<T>)collection.ToImmutableArray();
    }
}
