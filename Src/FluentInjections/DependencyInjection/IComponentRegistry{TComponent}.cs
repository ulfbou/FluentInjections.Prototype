// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Components;

using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection;

public interface IComponentRegistry<TComponent> where TComponent : IComponent
{
    IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>>? predicate,
        CancellationToken cancellationToken)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(
        IEnumerable<string> aliases,
        CancellationToken cancellationToken)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(
        string? alias = null,
        CancellationToken cancellationToken = default)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(
        Func<TDescriptor, ValueTask<bool>> predicate,
        CancellationToken cancellationToken)
            where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    Task RegisterAsync<TRegistration, TContract>(
        TRegistration descriptor,
        CancellationToken cancellationToken)
        where TRegistration : IComponentRegistration<TComponent, TContract>;
    Task RegisterAsync<TRegistration>(TRegistration registration, CancellationToken cancellationToken) where TRegistration : IComponentRegistration<TComponent>;
    ValueTask<bool> UnregisterAsync(string alias, CancellationToken cancellationToken = default);
}
