// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;
using FluentInjections.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace FluentInjections.Internal.Core
{
    //public sealed class ComponentResolver<TComponent> : IComponentResolver<TComponent>
    //    where TComponent : IComponent
    //{
    //    private readonly IComponentRegistry<TComponent> _registry;
    //    private readonly IConcurrentEventBus _eventBus;
    //    private readonly IAsyncConcurrentDictionary<string, object> _cache;

    //    public ComponentResolver(
    //        IComponentRegistry<TComponent> registry,
    //        IConcurrentEventBus eventBus,
    //        IAsyncConcurrentDictionary<string, object> cache)
    //    {
    //        _registry = registry;
    //        _eventBus = eventBus;
    //        _cache = cache;
    //    }

    //    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
    //        string? alias = null, CancellationToken cancellationToken = default)
    //    {
    //        if (alias is null) return default;

    //        if (_cache.ToList().TryGetValue(alias, out var cached) && cached is TContract cachedContract)
    //            return cachedContract;

    //        var descriptor = await _registry.GetSingleAsync<IComponentDescriptor<TComponent, TContract>, TContract>(alias, cancellationToken);
    //        if (descriptor is null) return default;

    //        var instance = descriptor.Resolve();
    //        if (instance is not null)
    //        {
    //            await _cache.SetAsync(alias, instance, cancellationToken);
    //            await _eventBus.PublishAsync(new ResolutionEvent<TComponent, TContract>(descriptor, instance));
    //        }

    //        return instance;
    //    }

    //    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
    //        Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
    //        CancellationToken cancellationToken = default)
    //    {
    //        await foreach (var descriptor in _registry.GetManyAsync(predicate, cancellationToken))
    //        {
    //            var instance = descriptor.Resolve();
    //            if (instance is not null)
    //            {
    //                await _eventBus.PublishAsync(new ResolutionEvent<TComponent, TContract>(descriptor, instance));
    //                return instance;
    //            }
    //        }
    //        return default;
    //    }

    //    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
    //        Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
    //        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    //    {
    //        var results = new List<TContract>();

    //        await foreach (var descriptor in _registry.GetManyAsync(predicate, cancellationToken))
    //        {
    //            var instance = descriptor.Resolve();
    //            if (instance is not null)
    //            {
    //                results.Add(instance);
    //                await _eventBus.PublishAsync(new ResolutionEvent<TComponent, TContract>(descriptor, instance));
    //            }
    //        }

    //        foreach (var result in results)
    //            yield return result;
    //    }

    //    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
    //        IEnumerable<string> aliases,
    //        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    //    {
    //        var results = new ConcurrentBag<TContract>();

    //        await Task.WhenAll(aliases.Select(async alias =>
    //        {
    //            var instance = await ResolveSingleAsync<TContract>(alias, cancellationToken);
    //            if (instance is not null) results.Add(instance);
    //        }));

    //        foreach (var result in results)
    //            yield return result;
    //    }
    //}
}
