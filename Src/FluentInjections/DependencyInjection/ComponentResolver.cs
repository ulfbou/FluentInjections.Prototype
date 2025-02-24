// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection;

public class ComponentResolver<TComponent> : IComponentResolver<TComponent>
    where TComponent : IComponent
{
    private readonly IComponentRegistry<TComponent> _registry;
    private readonly IServiceProvider _innerProvider;

    public Guid ScopeId { get; } = Guid.NewGuid();

    public ComponentResolver(IComponentRegistry<TComponent> registry, IServiceProvider innerProvider)
    {
        _registry = registry;
        _innerProvider = innerProvider;
    }

    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
        Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
        CancellationToken cancellationToken = default)
    {
        var descriptor = await _registry.GetSingleAsync<IComponentDescriptor<TComponent, TContract>, TContract>(
            predicate ?? (d => new ValueTask<bool>(true)),
            cancellationToken);

        return await ResolveSingleInternalAsync(descriptor).ConfigureAwait(false);
    }

    public async ValueTask<TContract?> ResolveSingleAsync<TContract>(
        string? alias = null,
        CancellationToken cancellationToken = default)
    {
        IComponentDescriptor<TComponent, TContract>? descriptor = await _registry.GetSingleAsync<IComponentDescriptor<TComponent, TContract>, TContract>(alias, cancellationToken).ConfigureAwait(false);
        return await ResolveSingleInternalAsync(descriptor).ConfigureAwait(false);
    }

    private async ValueTask<TContract?> ResolveSingleInternalAsync<TContract>(IComponentDescriptor<TComponent, TContract>? descriptor, CancellationToken cancellationToken = default)
    {
        if (descriptor is null)
        {
            return default(TContract?);
        }

        if (descriptor.Instance is not null)
        {
            return descriptor.Instance;
        }

        if (descriptor.Factory is not null)
        {
            return await descriptor.Factory(_innerProvider, cancellationToken);
        }

        try
        {
            return (TContract?)_innerProvider.GetService(typeof(TContract));
        }
        catch
        {
            return default(TContract?);
        }
    }

    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var descriptors = _registry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(predicate ?? (d => new ValueTask<bool>(true)), cancellationToken);

        await foreach (var descriptor in descriptors)
        {
            var instance = await ResolveSingleInternalAsync(descriptor, cancellationToken).ConfigureAwait(false);

            if (instance is not null)
            {
                yield return instance;
            }
        }
    }

    public async IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
        IEnumerable<string> aliases,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var descriptors = _registry.GetManyAsync<IComponentDescriptor<TComponent, TContract>, TContract>(aliases, cancellationToken);

        await foreach (var descriptor in descriptors)
        {
            var instance = await ResolveSingleInternalAsync(descriptor, cancellationToken).ConfigureAwait(false);

            if (instance is not null)
            {
                yield return instance;
            }
        }
    }

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}
