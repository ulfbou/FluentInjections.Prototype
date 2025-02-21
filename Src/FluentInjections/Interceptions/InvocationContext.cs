// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Reflection;

namespace FluentInjections.Interceptions;

public record struct InvocationContext(MethodInfo Method, object?[] Arguments, object Target, object? ReturnValue = null, Exception? Exception = null)
{
    private static readonly Lazy<MethodInfo> InvoceAsync = new(() => typeof(InterceptorPipeline).GetMethod(nameof(InterceptorPipeline.InvokeAsync), BindingFlags.NonPublic | BindingFlags.Instance)
        ?? throw new InvalidOperationException("Method not found"));

    public MethodInfo Method { get; } = Method ?? InvoceAsync.Value;
    public InvocationContext WithReturnValue(object? returnValue) => this with { ReturnValue = returnValue };
    public InvocationContext WithException(Exception? exception) => this with { Exception = exception };
}
