// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Validation;

using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

using FluentInjections.Components;

/// <inheritdoc/>

namespace FluentInjections.DependencyInjection;

internal sealed class ComponentRegistry<TComponent> : IComponentRegistry<TComponent>
    where TComponent : IComponent
{
    private readonly List<object> _descriptors = new List<object>();
    private readonly ILogger<ComponentRegistry<TComponent>> _logger;

    public ComponentRegistry(ILogger<ComponentRegistry<TComponent>> logger)
    {
        Guard.NotNull(logger, nameof(logger));
        _logger = logger;
    }

    public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>>? predicate = null,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        foreach (var descriptor in _descriptors.OfType<TDescriptor>())
        {
            if (predicate is null || await predicate(descriptor))
            {
                yield return descriptor;
            }
        }
    }

    public async IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        IEnumerable<string> aliases,
        [EnumeratorCancellation] CancellationToken cancellationToken)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        foreach (var descriptor in _descriptors.OfType<TDescriptor>())
        {
            if (aliases.Contains(descriptor.Alias))
                yield return descriptor;
        }

        await Task.CompletedTask;
    }

    public ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(
        string? alias = null,
        CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        return GetSingleAsync<TDescriptor, TContract>(x => ValueTask.FromResult(x.Alias == alias), cancellationToken);
    }

    public async ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>> predicate,
        CancellationToken cancellationToken)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>
    {
        foreach (var descriptor in _descriptors.OfType<TDescriptor>())
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await predicate(descriptor))
            {
                return descriptor;
            }
        }

        return default;
    }

    public async Task RegisterAsync<TRegistration, TContract>(
        TRegistration descriptor,
        CancellationToken cancellationToken)
            where TRegistration : IComponentRegistration<TComponent, TContract>
    {
        _descriptors.Add(descriptor);
        await Task.CompletedTask;
    }

    public async Task RegisterAsync<TRegistration>(
        TRegistration descriptor,
        CancellationToken cancellationToken)
            where TRegistration : IComponentRegistration<TComponent>
    {
        _descriptors.Add(descriptor);
        await Task.CompletedTask;
    }

    public async ValueTask<bool> UnregisterAsync(string alias, CancellationToken cancellationToken = default)
    {
        var toRemove = _descriptors
            .OfType<IComponentDescriptor<TComponent, object>>()
            .FirstOrDefault(d => d.Alias == alias);

        if (toRemove != null)
        {
            _descriptors.Remove(toRemove);
            return await Task.FromResult(true);
        }

        return await Task.FromResult(false);
    }
}
