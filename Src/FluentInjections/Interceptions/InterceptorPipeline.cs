// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Castle.DynamicProxy;

using FluentInjections.Validation;

using System.Collections.Concurrent;
using System.Reflection;

namespace FluentInjections.Interceptions;

public class InterceptorPipeline : IInterceptorPipeline
{
    private readonly List<(IInterceptor interceptor, int registrationOrder)> _interceptors = new();
    private readonly ConcurrentDictionary<Type, Delegate> _interceptDelegateCache = new();
    private readonly ConcurrentDictionary<MethodInfo, Delegate> _delegateCacheVoid = new();
    private readonly ConcurrentDictionary<MethodInfo, Delegate> _delegateCacheInt = new();
    private readonly ConcurrentDictionary<MethodInfo, Delegate> _delegateCacheString = new();
    private readonly ReaderWriterLockSlim _interceptorLock = new();
    private readonly IServiceProvider _serviceProvider;

    public InterceptorPipeline(IServiceProvider serviceProvider, IEnumerable<IInterceptor>? interceptors = null)
    {
        Guard.NotNull(serviceProvider, nameof(serviceProvider));
        _serviceProvider = serviceProvider;

        if (interceptors is not null)
        {
            _interceptors.AddRange(interceptors.Select((interceptor, index) => (interceptor, index)));
        }
    }

    public void AddInterceptor(IInterceptor interceptor)
    {
        _interceptorLock.EnterWriteLock();
        try
        {
            _interceptors.Add((interceptor, _interceptors.Count));
        }
        finally
        {
            _interceptorLock.ExitWriteLock();
        }
    }

    public async Task<object?> InvokeAsync(InvocationContext context, CancellationToken cancellationToken)
    {
        Func<InvocationContext, CancellationToken, ValueTask<object?>> nextInterceptor = InvokeTargetMethodAsync; // Start with the target method

        for (int i = _interceptors.Count - 1; i >= 0; i--)
        {
            var interceptor = _interceptors[i].interceptor;
            var currentNext = nextInterceptor; // Capture the current nextInterceptor

            if (interceptor.ShouldIntercept(context)) // Call ShouldIntercept here
            {
                if (interceptor is IInterceptorVoid voidInterceptor)
                {
                    nextInterceptor = async (ctx, ct) => { await voidInterceptor.InterceptAsync(ctx, NextInterceptorVoid, ct); return null; }; // Correctly use NextInterceptorVoid
                }
                else
                {
                    nextInterceptor = async (ctx, ct) => await interceptor.InterceptAsync(ctx, currentNext, ct); // Generic interceptor
                }
            }
        }

        return await nextInterceptor(context, cancellationToken).ConfigureAwait(false); // Call the chain, configure await
    }

    private async ValueTask NextInterceptorVoid(InvocationContext context, CancellationToken cancellationToken)
    {
        await InvokeTargetMethodAsync(context, cancellationToken); // Call the target method
    }

    private async ValueTask<object?> InvokeTargetMethodAsync(InvocationContext context, CancellationToken cancellationToken)
    {
        var methodDelegate = CreateDelegate(context.Method);
        var result = methodDelegate.DynamicInvoke(context, cancellationToken);
        if (result is Task taskResult)
        {
            await taskResult.ConfigureAwait(false);
            return taskResult.GetType().GetProperty("Result")?.GetValue(taskResult);
        }
        return result;
    }

    public Delegate CreateDelegate(MethodInfo methodInfo)
    {
        if (methodInfo is null)
        {
            methodInfo ??= typeof(InterceptorPipeline).GetMethod(nameof(InvokeAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new InvalidOperationException("Method 'InvokeAsync' not found.");
        }

        if (methodInfo.ReturnType == typeof(void))
        {
            return _delegateCacheVoid.GetOrAdd(methodInfo, mi => Delegate.CreateDelegate(typeof(Func<InvocationContext, CancellationToken, ValueTask>), mi));
        }
        else if (methodInfo.ReturnType == typeof(int))
        {
            return _delegateCacheInt.GetOrAdd(methodInfo, mi => Delegate.CreateDelegate(typeof(Func<InvocationContext, CancellationToken, ValueTask<int?>>), mi));
        }

        throw new NotSupportedException($"Return type '{methodInfo.ReturnType}' is not supported.");
    }
}
