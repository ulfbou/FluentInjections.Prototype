// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Async
{
    public interface IAsyncOperationManager
    {
        ValueTask<T> ExecuteAsync<T>(Func<CancellationToken, ValueTask<T>> taskFunction, int timeoutMilliseconds = Timeout.Infinite);
        ValueTask ExecuteAsync(Func<CancellationToken, ValueTask> taskFunction, int timeoutMilliseconds = Timeout.Infinite);
    }
}
