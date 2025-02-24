// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Interceptions
{
    public interface IInterceptorVoid
    {
        ValueTask InterceptAsync(InvocationContext context, Func<InvocationContext, CancellationToken, ValueTask> next, CancellationToken cancellationToken);
    }
}
