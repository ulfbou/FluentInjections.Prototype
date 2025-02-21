using System.Collections;

public class ReadOnlyAsyncEnumerable<T> : IReadOnlyAsyncEnumerable<T>
{
    private readonly IAsyncEnumerable<T> _source;

    // Determine Count property implementation
    public int Count
    {
        get
        {
            if (!_count.HasValue)
            {
                _count = _source switch
                {
                    IReadOnlyCollection<T> collection => collection.Count,
                    _ => CountAsync().ConfigureAwait(false).GetAwaiter().GetResult()
                };
            }

            return _count.Value;
        }
    }
    private int? _count;

    public ReadOnlyAsyncEnumerable(IAsyncEnumerable<T> source)
    {
        _source = source;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _source.GetAsyncEnumerator(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var count = 0;
        await foreach (var _ in _source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            count++;
        }
        return count;
    }

    public async IAsyncEnumerator<T> GetAsyncEnumerator()
    {
        await foreach (var item in _source)
        {
            yield return item;
        }
    }

    public IEnumerator<T> GetEnumerator() => throw new NotImplementedException();
    IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
