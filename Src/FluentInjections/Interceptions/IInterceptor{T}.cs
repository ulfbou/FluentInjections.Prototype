// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Interceptions
{
    public interface IInterceptor<T>
    {
        ValueTask<T?> InterceptAsync(InvocationContext context, Func<InvocationContext, CancellationToken, ValueTask<T?>> next, CancellationToken cancellationToken);
    }
}
