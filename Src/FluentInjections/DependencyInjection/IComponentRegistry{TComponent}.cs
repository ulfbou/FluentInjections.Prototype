// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection;

public interface IComponentRegistry<TComponent> where TComponent : IComponent
{
    IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(Func<TDescriptor, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default) where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(IEnumerable<string> aliases, CancellationToken cancellationToken = default) where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(string? alias = null, CancellationToken cancellationToken = default) where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(Func<TDescriptor, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) where TDescriptor : IComponentDescriptor<TComponent, TContract>;
    ValueTask RegisterAsync<TDescriptor>(TDescriptor descriptor, CancellationToken cancellationToken = default) where TDescriptor : IComponentDescriptor<TComponent>;
    ValueTask<bool> UnregisterAsync(string alias, CancellationToken cancellationToken = default);
}
