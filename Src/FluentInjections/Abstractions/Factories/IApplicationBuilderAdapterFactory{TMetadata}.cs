// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;
using FluentInjections.Abstractions.Metadata;
using FluentInjections.DependencyInjection;

namespace FluentInjections.Abstractions.Factories
{
    /// <summary>
    /// Factory for creating concrete builder adapters.
    /// </summary>
    public interface IApplicationBuilderAdapterFactory<TMetadata>
        where TMetadata : IApplicationTypeMetadata
    {
        // Example constructor: IApplicationBuilderAdapterFactory(IComponentResolverProvider resolverProvider)

        /// <summary>
        /// Creates a framework-agnostic application builder adapter asynchronously.
        /// </summary>
        /// <typeparam name="TConcreteBuilder">The type of the concrete builder.</typeparam>
        /// <typeparam name="TApplicationAbstraction">The type of the application abstraction.</typeparam>
        /// <param name="metadata">The metadata for the application type.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation, resulting in an instance of IApplicationBuilderAdapter.</returns>
        Task<IApplicationBuilderAdapter<TConcreteBuilder, TApplicationAbstraction>> CreateAdapterAsync<TConcreteBuilder, TApplicationAbstraction>(
                TMetadata metadata,
                // Moved IComponentResolverProvider to the constructor
                CancellationToken cancellationToken = default)
            where TConcreteBuilder : IApplicationBuilderAbstraction
            where TApplicationAbstraction : IApplicationAbstraction;

        /// <summary>
        /// Creates a framework-agnostic IApplication instance asynchronously, fully configured with adapters and the concrete application.
        /// </summary>
        /// <typeparam name="TConcreteApplication">The type of the concrete application.</typeparam>
        /// <typeparam name="TApplicationAbstraction">The type of the application abstraction.</typeparam>
        /// <param name="builder">The framework-agnostic builder abstraction instance.</param>
        /// <param name="concreteApplication">The built concrete application instance (framework-specific).</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation, resulting in a framework-agnostic IApplication instance.</returns>
        Task<IApplicationAbstraction> CreateApplicationAsync<TConcreteApplication, TApplicationAbstraction>(
                IApplicationBuilderAdapter<IApplicationBuilderAbstraction, TApplicationAbstraction> builder,
                TConcreteApplication concreteApplication,
                CancellationToken cancellationToken = default)
            where TApplicationAbstraction : IApplicationAbstraction;
    }
}
