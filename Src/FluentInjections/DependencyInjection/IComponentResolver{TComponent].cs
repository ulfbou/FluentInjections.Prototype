// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Runtime.CompilerServices;

namespace FluentInjections.DependencyInjection;

public interface IComponentResolver<TComponent> where TComponent : IComponent
{
    Guid ScopeId { get; }

    ValueTask DisposeAsync();
    IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(IEnumerable<string> aliases, CancellationToken cancellationToken = default);
    IAsyncEnumerable<TContract> ResolveManyAsyncSimple<TContract>(Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default);
    ValueTask<TContract?> ResolveSingleAsync<TContract>(Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default);
    ValueTask<TContract?> ResolveSingleAsync<TContract>(string? alias = null, CancellationToken cancellationToken = default);
}
