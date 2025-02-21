// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Interceptions;

namespace FluentInjections;

public interface IInterceptor
{
    // Determines whether to apply the interceptor based on the InvocationContext.
    bool ShouldIntercept(InvocationContext context); // Method instead of property

    // Main interception method.
    ValueTask<object?> InterceptAsync(InvocationContext context, Func<InvocationContext, CancellationToken, ValueTask<object?>> nextInterceptor, CancellationToken cancellationToken);
}
