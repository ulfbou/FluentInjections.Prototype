// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Descriptors;
using FluentInjections.Events;
using FluentInjections.Interceptions;
using FluentInjections.Validation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Nito.AsyncEx;

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;

namespace FluentInjections.DependencyInjection;

public class ComponentResolver<TComponent>
    : ComponentResolverBase<TComponent>, IComponentResolver<TComponent> where TComponent : IComponent
{
    protected readonly IComponentRegistry<TComponent> _componentRegistry;
    protected readonly IInterceptorPipeline _interceptorPipeline;
    protected readonly Events.IConcurrentEventBus _eventBus;
    protected readonly ConcurrentDictionary<(Type, string?, Guid), object?> _resolutionCache = new();
    protected readonly ConcurrentDictionary<Guid, HashSet<(Type ContractType, string? Alias)>> _scopeCacheKeys = new();
    protected readonly ConcurrentDictionary<Type, ImmutableList<Func<object, ValueTask>>> _disposeHandlers = new();
    protected readonly AsyncLock _asyncLock = new AsyncLock();

    public Guid ScopeId { get; private set; }

    public ComponentResolver(
        IComponentRegistry<TComponent> componentRegistry,
        IEnumerable<IInterceptor> interceptors,
        Events.IConcurrentEventBus eventBus,
        IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _componentRegistry = componentRegistry ?? throw new ArgumentNullException(nameof(componentRegistry));
        _interceptorPipeline = new InterceptorPipeline(serviceProvider, interceptors);
        _eventBus = eventBus;

        _eventBus.SubscribeAsync<ScopeDisposedEvent>(async ev =>
        {
            try
            {
                await HandleScopeDisposedEventAsync((ScopeDisposedEvent)ev);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling ScopeDisposedEvent.");
                Debug.WriteLine(ex, "Error handling ScopeDisposedEvent.");
            }
        });

        if (serviceProvider is IServiceProviderWrapper serviceProviderWrapper)
        {
            ScopeId = serviceProviderWrapper.ScopeId;
        }
    }

    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
        Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linkedTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

        var descriptors = _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(predicate, linkedTokenSource.Token);

        await foreach (var descriptor in descriptors.WithCancellation(linkedTokenSource.Token))
        {
            var key = GetCacheKey(descriptor.ContractType, descriptor.Alias);

            using (await _asyncLock.LockAsync(linkedTokenSource.Token))
            {
                if (_resolutionCache.TryGetValue(key, out var instance))
                {
                    _logger.LogDebug("Component '{type}' with alias '{alias}' in scope '{scopeId}' retrieved from cache.", key.ContractType, key.Alias, key.ScopeId);
                    return (TContract?)instance;
                }

                instance = await ResolveAndCacheAsync<TContract>(key, linkedTokenSource.Token);

                if (instance != null)
                {
                    _logger.LogInformation("Component '{type}' with alias '{alias}' and scope '{scopeId}' resolved and cached.", key.ContractType, key.Alias, key.ScopeId);
                    Debug.WriteLine("Component '{type}' with alias '{alias}' and scope '{scopeId}' resolved and cached.", key.ContractType, key.Alias, key.ScopeId);
                    return (TContract?)instance;
                }
            }
        }

        _logger.LogWarning("No component matching the given predicate was found.");
        Debug.WriteLine("No component matching the given predicate was found.");
        return default;
    }

    private (Type, string?, Guid) GetCacheKey(object contractType, string alias) => throw new NotImplementedException();

    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(string? alias = null, CancellationToken cancellationToken = default)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linkedTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

        var key = GetCacheKey(typeof(TContract), alias);

        if (_resolutionCache.TryGetValue(key, out var cachedInstance))
        {
            _logger.LogDebug("Component '{type}' with alias '{alias}' in scope '{scopeId}' retrieved from cache.", key.ContractType, key.Alias, key.ScopeId);
            return (TContract?)cachedInstance;
        }

        try
        {
            using (await _asyncLock.LockAsync(linkedTokenSource.Token))
            {
                if (_resolutionCache.TryGetValue(key, out cachedInstance))
                {
                    _logger.LogDebug("Component '{type}' with alias '{alias}' in scope '{scopeId}' retrieved from cache.", key.ContractType, key.Alias, key.ScopeId);
                    return (TContract?)cachedInstance;
                }

                return await ResolveAndCacheAsync<TContract>(key, linkedTokenSource.Token);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Component resolution timed out after 30 seconds for type '{type}' with alias '{alias}'.", key.ContractType, key.Alias);
            Debug.WriteLine("Component resolution timed out after 30 seconds for type '{type}' with alias '{alias}'.", key.ContractType, key.Alias);
            return default;
        }
        catch
        {
            return default;
        }
    }

    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
        Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linkedTokenSource.CancelAfter(TimeSpan.FromSeconds(30));

        var channel = Channel.CreateUnbounded<TContract>();

        IAsyncEnumerable<IComponentDescriptor<TComponent, TContract>> descriptors = _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(predicate, linkedTokenSource.Token);

        await foreach (var descriptor in descriptors)
        {
            var key = GetCacheKey(descriptor.ContractType, descriptor.Alias);

            using (await _asyncLock.LockAsync(linkedTokenSource.Token))
            {
                if (!_resolutionCache.TryGetValue(key, out var instance))
                {
                    instance = await ResolveAndCacheAsync<TContract>(key, linkedTokenSource.Token);

                    if (instance == null)
                    {
                        continue;
                    }
                }

                await channel.Writer.WriteAsync((TContract)instance!, linkedTokenSource.Token);
            }
        }

        channel.Writer.Complete();

        await foreach (var item in channel.Reader.ReadAllAsync(linkedTokenSource.Token))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
        IEnumerable<string> aliases,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var channel = Channel.CreateUnbounded<TContract>();

        foreach (var alias in aliases)
        {
            var key = GetCacheKey(typeof(TContract), alias);
            using (await _asyncLock.LockAsync(cancellationToken))
            {
                if (!_resolutionCache.TryGetValue(key, out var instance))
                {
                    instance = await ResolveAndCacheAsync<TContract>(key, cancellationToken);
                    if (instance == null)
                    {
                        continue;
                    }
                }
                await channel.Writer.WriteAsync((TContract)instance!, cancellationToken);
            }
        }
        channel.Writer.Complete();
        await foreach (var item in channel.Reader.ReadAllAsync(cancellationToken))
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<TContract> ResolveManyAsyncSimple<TContract>(
            Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IAsyncEnumerable<IComponentDescriptor<TComponent, TContract>> descriptors = _componentRegistry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(predicate, cancellationToken);

        await foreach (var descriptor in descriptors)
        {
            var key = GetCacheKey(descriptor.ContractType, descriptor.Alias);

            using (await _asyncLock.LockAsync(cancellationToken))
            {
                var cachedInstance = _resolutionCache.TryGetValue(key, out var instance);

                if (!cachedInstance)
                {
                    instance = await ResolveAndCacheAsync<TContract>(key, cancellationToken);

                    if (instance == null)
                    {
                        continue;
                    }
                }

                yield return (TContract)instance!;
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        using (await _asyncLock.LockAsync())
        {
            foreach (var instance in _resolutionCache.Values)
            {
                if (instance == null)
                {
                    _logger.LogWarning("Null instance found in resolution cache during disposal. This might indicate a bug.");
                    Debug.WriteLine("Null instance found in resolution cache during disposal. This might indicate a bug.");
                    continue;
                }

                try
                {
                    var type = instance.GetType();

                    if (_disposeHandlers.TryGetValue(type, out var handlers))
                    {
                        foreach (var handler in handlers)
                        {
                            await handler(instance);
                        }
                    }

                    switch (instance)
                    {
                        case IAsyncDisposable asyncDisposable:
                            await asyncDisposable.DisposeAsync();
                            break;
                        case IDisposable disposable:
                            disposable.Dispose();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disposing components.");
                    Debug.WriteLine(ex, "Error disposing components.");
                }
            }

            _resolutionCache.Clear();
            _disposeHandlers.Clear();
            _scopeCacheKeys.Clear();
        }
    }

    protected async Task<TContract?> ResolveAndCacheAsync<TContract>((Type ContractType, string Alias, Guid ScopeId) key, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("Resolving component {ContractType} with alias {Alias}", key.ContractType, key.Alias))
        {
            IComponentDescriptor<TComponent, object>? descriptor = await _componentRegistry.GetSingleAsync<IComponentDescriptor<TComponent, object>, object>(key.Alias, cancellationToken);

            if (descriptor is null)
            {
                _logger.LogWarning("Unable to resolve component {ContractType} with alias {Alias}", key.ContractType, key.Alias);
                Debug.WriteLine("Unable to resolve component {ContractType} with alias {Alias}", key.ContractType, key.Alias);
                return default;
            }

            object? instance = await CreateInstanceAsync<TContract>(key, (ComponentDescriptor<TComponent, object>)descriptor, cancellationToken);

            if (instance == null)
            {
                return default;
            }

            await _eventBus.PublishAsync<ComponentResolvedEvent>(new ComponentResolvedEvent(key.ContractType, key.Alias, instance), TimeSpan.FromSeconds(30), cancellationToken);

            _resolutionCache[key] = instance;
            AddCacheEntry((key.ContractType, key.Alias), key.ScopeId);

            if (instance is TContract contract)
            {
                return contract;
            }

            return default;
        }
    }

    protected async ValueTask<object?> ResolveAndCacheAsync((Type ContractType, string Alias, Guid ScopeId) key, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("Resolving component {ContractType} with alias {Alias}", key.ContractType, key.Alias))
        {
            IComponentDescriptor<TComponent, object>? descriptor = await _componentRegistry.GetSingleAsync<IComponentDescriptor<TComponent, object>, object>(key.Alias, cancellationToken);

            if (descriptor is null)
            {
                _logger.LogWarning("Unable to resolve component {ContractType} with alias {Alias}", key.ContractType, key.Alias);
                Debug.WriteLine("Unable to resolve component {ContractType} with alias {Alias}", key.ContractType, key.Alias);
                return null;
            }

            object? instance = await CreateInstanceAsync<object>(key, (ComponentDescriptor<TComponent, object>)descriptor, cancellationToken);

            if (instance == null)
            {
                return null;
            }

            await _eventBus.PublishAsync<ComponentResolvedEvent>(new ComponentResolvedEvent(key.ContractType, key.Alias, instance), TimeSpan.FromSeconds(30), cancellationToken);

            _resolutionCache[key] = instance;
            AddCacheEntry((key.ContractType, key.Alias), key.ScopeId);
            return instance;
        }
    }

    protected async Task<object?> CreateInstanceAsync<TContract>((Type ContractType, string Alias, Guid ScopeId) key, ComponentDescriptor<TComponent, object> descriptor, CancellationToken cancellationToken)
    {
        Guard.NotNull(key, nameof(key));
        Guard.NotNull(descriptor, nameof(descriptor));

        object? instance = descriptor.Instance;

        if (instance is null)
        {
            if (descriptor.Factory is not null)
            {
                instance = descriptor.Factory?.Invoke(ServiceProvider);
            }

            if (instance is null)
            {
                try
                {
                    if (descriptor.ResolutionType is not null)
                    {
                        instance = Activator.CreateInstance(descriptor.ResolutionType);
                    }
                    else
                    {
                        instance = Activator.CreateInstance(descriptor.ContractType);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating instance of type '{type}' with alias '{alias}'.", descriptor.ContractType, descriptor.Alias);
                    Debug.WriteLine("{Exception} Error creating instance of type '{type}' with alias '{alias}'.", ex.Message, descriptor.ContractType, descriptor.Alias);
                    return default;
                }
            }
        }

        var invocationContext = new InvocationContext(
            null!,
            Array.Empty<object?>(),
            key.ContractType);
        var interceptedInstance = await _interceptorPipeline.InvokeAsync(invocationContext, cancellationToken);

        var targetType = invocationContext.Target.GetType();
        var contractType = key.ContractType;
        var interceptedType = interceptedInstance?.GetType();

        Debug.WriteLine($"Target type: {targetType}");
        Debug.WriteLine($"Contract type: {contractType}");
        Debug.WriteLine($"Intercepted type: {interceptedType}");

        // KEY CHANGE: Prioritize the original instance if it's the correct type
        if (invocationContext.Target.GetType() == key.ContractType) // Check if the target type matches the contract type
        {
            return invocationContext.Target; // Use the instance from the context
        }
        else if (interceptedInstance != null && interceptedInstance.GetType() == key.ContractType) // Fallback: Check the original result
        {
            return interceptedInstance;
        }
        else if (interceptedInstance != null && key.ContractType.IsAssignableFrom(interceptedInstance.GetType()))
        {
            return interceptedInstance; // Cast if type is assignable
        }

        // Handle the case where the interceptor didn't set the Target correctly
        throw new InvalidCastException($"Cannot cast '{interceptedInstance?.GetType().Name ?? "null"}' to '{typeof(TContract).Name}'.");
    }

    protected (Type ContractType, string Alias, Guid ScopeId) GetCacheKey(Type contractType, string? alias)
    {
        Guard.NotNull(contractType, nameof(contractType));

        Guid scopeId = Guid.Empty;

        if (ServiceProvider is IServiceProviderWrapper serviceProviderWrapper)
        {
            scopeId = serviceProviderWrapper.ScopeId;
        }

        return (contractType, alias ?? contractType.FullName ?? contractType.Name, scopeId);
    }

    protected void AddCacheEntry((Type, string?) key, Guid scopeId)
    {
        if (!_scopeCacheKeys.ContainsKey(scopeId))
        {
            _scopeCacheKeys[scopeId] = new HashSet<(Type ContractType, string? Alias)>();
        }

        _scopeCacheKeys[scopeId].Add(key);
    }

    protected async Task HandleScopeDisposedEventAsync(ScopeDisposedEvent ev)
    {
        if (IsRelevantScope(ev.ScopeId))
        {
            await InvalidateScopeCache(ev.ScopeId);
            _logger.LogInformation("Cache invalidated for scope '{scopeId}'.", ev.ScopeId);
            Debug.WriteLine("Cache invalidated for scope '{scopeId}'.", ev.ScopeId);
        }
    }

    protected bool IsRelevantScope(Guid scopeId)
    {
        return _scopeCacheKeys.ContainsKey(scopeId);
    }

    protected Task InvalidateScopeCache(Guid scopeId)
    {
        if (_scopeCacheKeys.TryRemove(scopeId, out var keys))
        {
            foreach (var key in keys)
            {
                var cacheKey = (key.ContractType, key.Alias, scopeId);
                _resolutionCache.TryRemove(cacheKey, out _);
            }
        }

        return Task.CompletedTask;
    }
}
