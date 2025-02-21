// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.


namespace FluentInjections.Events;

public interface IConcurrentEventHandlerInvoker
{
    Task InvokeHandlerAsync<TEvent>(Func<TEvent, ValueTask> handler, TEvent @event, TimeSpan timeout, CancellationToken cancellationToken);
}
