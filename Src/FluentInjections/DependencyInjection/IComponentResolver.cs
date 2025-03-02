// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.DependencyInjection
{
    /// <summary>
    /// Represents a component resolver for resolving component instances.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    public interface IComponentResolver<TComponent> : IAsyncDisposable where TComponent : IComponent
    {
        /// <summary>
        /// Gets the component registry associated with this resolver.
        /// </summary>
        IComponentRegistry<TComponent> ComponentRegistry { get; }

        /// <summary>
        /// Asynchronously resolves multiple component instances based on a predicate.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="predicate">An optional predicate to filter the components.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of component instances.</returns>
        IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
                Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
                CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously resolves multiple component instances based on a list of aliases.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="aliases">A collection of aliases to filter the components.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of component instances.</returns>
        IAsyncEnumerable<TContract> ResolveManyAsync<TContract>(
            IEnumerable<string> aliases,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously resolves multiple component instances based on a predicate, without descriptor information.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="predicate">An optional predicate to filter the components.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of component instances.</returns>
        IAsyncEnumerable<TContract> ResolveManyAsyncSimple<TContract>(
            Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously resolves a single component instance based on a predicate.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="predicate">A predicate to filter the component.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask{TResult?}"/> representing the asynchronous operation, resulting in a component instance or null.</returns>
        ValueTask<TContract?> ResolveSingleAsync<TContract>(
            Func<IComponentDescriptor<TComponent, TContract>, ValueTask<bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously resolves a single component instance based on an alias.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="alias">The alias of the component.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask{TResult?}"/> representing the asynchronous operation, resulting in a component instance or null.</returns>
        ValueTask<TContract?> ResolveSingleAsync<TContract>(
            string? alias = null,
            CancellationToken cancellationToken = default);
    }
}
