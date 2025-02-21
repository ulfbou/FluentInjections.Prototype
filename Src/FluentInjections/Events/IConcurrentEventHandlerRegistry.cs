// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Immutable;

namespace FluentInjections.Events;

public interface IConcurrentEventHandlerRegistry
{
    Task AddAsync<TEvent>(Func<TEvent, ValueTask> handler);
    Task CleanupCollectedHandlersAsync();
    ValueTask DisposeAsync();
    Task<ImmutableList<WeakReference<Delegate>>> GetHandlersAsync<TEvent>();
    Task<bool> RemoveAsync<TEvent>(Func<TEvent, ValueTask> handler);
    Task<bool> RemoveAsync<TEvent>(Func<TEvent, ValueTask> handler, object context);
}
