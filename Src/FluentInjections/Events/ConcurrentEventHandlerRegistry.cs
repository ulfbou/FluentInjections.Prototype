using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentInjections.Events;

public class ConcurrentEventHandlerRegistry : IConcurrentEventHandlerRegistry
{
    private readonly ConcurrentDictionary<Type, ImmutableList<WeakReference<Func<object, ValueTask>>>> _eventHandlers = new();
    private readonly ConcurrentDictionary<Type, ImmutableList<Delegate>> _typedEventHandlers = new();
    private readonly ILogger<ConcurrentEventHandlerRegistry> _logger;
    private readonly AsyncLock _asyncLock = new();
    private readonly Timer _cleanupTimer;
    private bool _disposed;

    public ConcurrentEventHandlerRegistry(ILogger<ConcurrentEventHandlerRegistry> logger, TimeSpan cleanupInterval)
    {
        _logger = logger;
        _cleanupTimer = new Timer(async _ => await CleanupCollectedHandlersAsync(), null, cleanupInterval, cleanupInterval);
    }

    public async Task AddAsync<TEvent>(Func<TEvent, ValueTask> handler)
    {
        var key = typeof(TEvent);
        var objectHandler = (Func<object, ValueTask>)(async @event => await handler((TEvent)@event).ConfigureAwait(false));

        using (await _asyncLock.LockAsync())
        {
            _eventHandlers.AddOrUpdate(key,
                _ => ImmutableList<WeakReference<Func<object, ValueTask>>>.Empty.Add(new WeakReference<Func<object, ValueTask>>(objectHandler)),
                (_, existingHandlers) => existingHandlers.Add(new WeakReference<Func<object, ValueTask>>(objectHandler)));

            _typedEventHandlers.AddOrUpdate(key,
                _ => ImmutableList<Delegate>.Empty.Add(handler),
                (_, existingHandlers) => existingHandlers.Add(handler));
        }

        _logger.LogInformation("Handler for event {EventType} added.", key);
    }

    public async Task<bool> RemoveAsync<TEvent>(Func<TEvent, ValueTask> handler)
    {
        return await RemoveInternalAsync(handler, null);
    }

    public async Task<bool> RemoveAsync<TEvent>(Func<TEvent, ValueTask> handler, object context)
    {
        return await RemoveInternalAsync(handler, context);
    }

    private async Task<bool> RemoveInternalAsync<TEvent>(Func<TEvent, ValueTask> handler, object? context)
    {
        var key = typeof(TEvent);
        var objectHandlerToRemove = (Func<object, ValueTask>)(async @event => await handler((TEvent)@event).ConfigureAwait(false));

        using (await _asyncLock.LockAsync())
        {
            var removed = _typedEventHandlers.AddOrUpdate(key,
                _ => ImmutableList<Delegate>.Empty,
                (_, existingHandlers) => existingHandlers.RemoveAll(h => h.Equals(handler) && (context == null || (h is Delegate del && del.Target == context)))).Count > 0;

            if (removed)
            {
                _eventHandlers.AddOrUpdate(key,
                    _ => ImmutableList<WeakReference<Func<object, ValueTask>>>.Empty,
                    (_, existingHandlers) => existingHandlers.RemoveAll(wr => wr.TryGetTarget(out var target) && target.Equals(objectHandlerToRemove)));
                _logger.LogInformation("Handler for event {EventType} removed{ContextInfo}.", key, context == null ? "" : $" with context {context}");
            }

            return removed;
        }
    }

    public async Task<ImmutableList<WeakReference<Delegate>>> GetHandlersAsync<TEvent>()
    {
        var key = typeof(TEvent);

        using (await _asyncLock.LockAsync())
        {
            var handlers = _typedEventHandlers.TryGetValue(key, out var delegates) ? delegates.Select(d => new WeakReference<Delegate>(d)).ToImmutableList() : ImmutableList<WeakReference<Delegate>>.Empty;
            _logger.LogDebug("Retrieved handlers for event {EventType}: {HandlerCount}", key, handlers.Count);
            return handlers;
        }
    }

    public async Task CleanupCollectedHandlersAsync()
    {
        using (await _asyncLock.LockAsync())
        {
            foreach (var key in _eventHandlers.Keys.ToList())
            {
                _eventHandlers.AddOrUpdate(key,
                    _ => ImmutableList<WeakReference<Func<object, ValueTask>>>.Empty,
                    (_, existingHandlers) => existingHandlers.RemoveAll(wr =>
                    {
                        bool hasTarget = wr.TryGetTarget(out var target);
                        return !hasTarget;
                    }));
            }
        }

        _logger.LogInformation("Cleaned up collected handlers.");
    }

    public ValueTask DisposeAsync()
    {
        if (_disposed)
        {
            return ValueTask.CompletedTask;
        }

        _disposed = true;
        _cleanupTimer.Dispose();
        return ValueTask.CompletedTask;
    }
}
