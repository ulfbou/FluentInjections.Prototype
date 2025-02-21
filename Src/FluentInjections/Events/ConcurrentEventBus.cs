// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Events;
using FluentInjections;

using Microsoft.Extensions.Logging;

using System.Collections.Concurrent;

namespace FluentInjections.Events;

public class ConcurrentEventBus : IConcurrentEventBus
{
    private readonly IConcurrentEventHandlerRegistry _handlerRegistry;
    private readonly IConcurrentEventHandlerInvoker _invoker;
    private readonly ILogger<ConcurrentEventBus> _logger;
    private readonly CancellationTokenSource _disposalCts = new();
    private readonly SemaphoreSlim _publishSemaphore;
    private readonly int _maxConcurrentHandlers;
    private readonly int _maxConcurrentPublishes;

    public ConcurrentEventBus(
        IConcurrentEventHandlerRegistry handlerRegistry,
        IConcurrentEventHandlerInvoker invoker,
        ILogger<ConcurrentEventBus> logger,
        int maxConcurrentHandlers = 8,
        int maxConcurrentPublishes = 4)
    {
        _handlerRegistry = handlerRegistry ?? throw new ArgumentNullException(nameof(handlerRegistry));
        _invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _maxConcurrentHandlers = maxConcurrentHandlers;
        _maxConcurrentPublishes = maxConcurrentPublishes;
        _publishSemaphore = new SemaphoreSlim(_maxConcurrentPublishes);
    }

    public ValueTask DisposeAsync() => _handlerRegistry.DisposeAsync();

    public async Task PublishAsync<TEvent>(TEvent @event, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        await _publishSemaphore.WaitAsync(cancellationToken);
        try
        {
            var eventType = typeof(TEvent);
            var handlers = await _handlerRegistry.GetHandlersAsync<TEvent>().ConfigureAwait(false);

            if (handlers.Count > 0)
            {
                _logger.LogInformation("Publishing event of type {EventType} to {HandlerCount} handlers", eventType, handlers.Count);

                var tasks = new List<Task>();
                var handlerSemaphore = new SemaphoreSlim(_maxConcurrentHandlers);

                foreach (var hr in handlers)
                {
                    if (hr.TryGetTarget(out var @delegate))
                    {
                        if (@delegate is Func<TEvent, ValueTask> handler)
                        {
                            await handlerSemaphore.WaitAsync(cancellationToken);

                            tasks.Add(_invoker.InvokeHandlerAsync(handler, @event, timeout, cancellationToken).ContinueWith(_ => handlerSemaphore.Release()));
                        }
                        else
                        {
                            _logger.LogWarning("Handler for event {EventType} is not of type Func<TEvent, ValueTask>. Skipping.", eventType);
                        }
                    }
                }

                try
                {
                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "One or more handlers for {EventType} failed", eventType);
                }

                await _handlerRegistry.CleanupCollectedHandlersAsync().ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("No handlers registered for event of type {EventType}", eventType);
            }
        }
        finally
        {
            _publishSemaphore.Release();
        }
    }

    public async Task SubscribeAsync<TEvent>(Func<TEvent, ValueTask> handler)
    {
        await _handlerRegistry.AddAsync(handler);
    }

    public async Task UnsubscribeAsync<TEvent>(Func<TEvent, ValueTask> handler)
    {
        await _handlerRegistry.RemoveAsync(handler);
    }
}
