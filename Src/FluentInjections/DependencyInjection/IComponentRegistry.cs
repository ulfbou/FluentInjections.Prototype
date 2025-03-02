// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.DependencyInjection
{
    /// <summary>
    /// Represents a component registry for managing component descriptors.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    public interface IComponentRegistry<TComponent> where TComponent : Abstractions.IComponent
    {
        /// <summary>
        /// Asynchronously retrieves multiple component descriptors based on a predicate.
        /// </summary>
        /// <typeparam name="TDescriptor">The type of the component descriptor.</typeparam>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="predicate">An optional predicate to filter the descriptors.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of component descriptors.</returns>
        IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(Func<TDescriptor, CancellationToken, ValueTask<bool>>? predicate = null, CancellationToken cancellationToken = default) where TDescriptor : Abstractions.IComponentDescriptor<TComponent, TContract>;

        /// <summary>
        /// Asynchronously retrieves multiple component descriptors based on a list of aliases.
        /// </summary>
        /// <typeparam name="TDescriptor">The type of the component descriptor.</typeparam>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="aliases">A collection of aliases to filter the descriptors.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>An <see cref="IAsyncEnumerable{T}"/> of component descriptors.</returns>
        IAsyncEnumerable<TDescriptor> GetManyAsync<TDescriptor, TContract>(IEnumerable<string> aliases, CancellationToken cancellationToken = default) where TDescriptor : Abstractions.IComponentDescriptor<TComponent, TContract>;

        /// <summary>
        /// Asynchronously retrieves a single component descriptor based on an alias.
        /// </summary>
        /// <typeparam name="TDescriptor">The type of the component descriptor.</typeparam>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="alias">The alias of the component descriptor.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation, resulting in a component descriptor or null.</returns>
        ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(string? alias = null, CancellationToken cancellationToken = default) where TDescriptor : Abstractions.IComponentDescriptor<TComponent, TContract>;

        /// <summary>
        /// Asynchronously retrieves a single component descriptor based on a predicate.
        /// </summary>
        /// <typeparam name="TDescriptor">The type of the component descriptor.</typeparam>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="predicate">A predicate to filter the descriptor.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask{TResult?}"/> representing the asynchronous operation, resulting in a component descriptor or null.</returns>
        ValueTask<TDescriptor?> GetSingleAsync<TDescriptor, TContract>(Func<TDescriptor, CancellationToken, ValueTask<bool>> predicate, CancellationToken cancellationToken = default) where TDescriptor : Abstractions.IComponentDescriptor<TComponent, TContract>;

        /// <summary>
        /// Asynchronously registers a component descriptor.
        /// </summary>
        /// <typeparam name="TRegistration">The type of the component descriptor.</typeparam>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="descriptor">The component descriptor to register.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        ValueTask RegisterAsync<TRegistration, TContract>(TRegistration descriptor, CancellationToken cancellationToken = default)
            where TRegistration : IComponentRegistration<TComponent, TContract>;

        /// <summary>
        /// Asynchronously unregisters a component descriptor based on an alias.
        /// </summary>
        /// <param name="alias">The alias of the component descriptor to unregister.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, resulting in a boolean indicating if the component descriptor was unregistered.</returns>
        ValueTask<bool> UnregisterAsync(string alias, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the <see cref="IComponentResolver{TComponent}"/> for resolving components.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A <see cref="ValueTask{TResult}"/> representing the asynchronous operation, resulting in a component resolver.</returns>
        ValueTask<IServiceProvider> GetServiceProviderAsync(CancellationToken cancellationToken = default);
    }
}
