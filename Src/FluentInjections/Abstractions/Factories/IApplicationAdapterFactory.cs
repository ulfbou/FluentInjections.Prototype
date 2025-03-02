// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

namespace FluentInjections.Abstractions.Factories
{
    /// <summary>
    /// Factory for creating concrete builder and application adapters.
    /// </summary>
    public interface IApplicationAdapterFactory // Should this be generic?
    {
        //// Example constructor: DefaultApplicationAdapterFactory(IComponentResolverProvider resolverProvider)

        ///// <summary>
        ///// Creates a concrete builder adapter for the target framework.
        ///// </summary>
        ///// <returns>An instance of IConcreteBuilderAdapter.</returns>
        //IConcreteBuilderAdapter CreateConcreteBuilderAdapter();

        ///// <summary>
        ///// Creates a concrete application adapter for the target framework.
        ///// </summary>
        ///// <returns>An instance of <see cref="IConcreteApplicationAdapter"/>.</returns>
        //IConcreteApplicationAdapter CreateConcreteApplicationAdapter();

        ///// <summary>
        ///// Creates a framework-agnostic IApplication instance, fully configured with adapters and the concrete application.
        ///// </summary>
        ///// <typeparam name="TBuilder">The type of the framework-agnostic builder abstraction.</typeparam>
        ///// <param name="builder">The framework-agnostic builder abstraction instance.</param>
        ///// <param name="concreteApplication">The built concrete application instance (framework-specific).</param>
        ///// <returns>A framework-agnostic IApplication instance.</returns>
        //IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder, object concreteApplication)
        //    where TBuilder : IApplicationBuilderAbstraction;
    }
}
