// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

namespace FluentInjections.Interceptions;

public interface IInterceptorPipeline
{
    void AddInterceptor(IInterceptor interceptor);
    Delegate CreateDelegate(MethodInfo methodInfo);
    Task<object?> InvokeAsync(InvocationContext context, CancellationToken cancellationToken);
}
