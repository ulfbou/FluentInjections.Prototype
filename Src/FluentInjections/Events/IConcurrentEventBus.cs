// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Events;

public interface IConcurrentEventBus : IAsyncDisposable
{
    Task PublishAsync<TEvent>(TEvent @event, TimeSpan timeout, CancellationToken cancellationToken = default);
    Task SubscribeAsync<TEvent>(Func<TEvent, ValueTask> handler);
    Task UnsubscribeAsync<TEvent>(Func<TEvent, ValueTask> handler);
}
