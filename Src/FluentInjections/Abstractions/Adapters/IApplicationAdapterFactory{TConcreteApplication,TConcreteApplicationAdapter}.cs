// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    /// <summary>
    /// Factory for creating concrete builder and application adapters.
    /// </summary>
    /// <typeparam name="TConcreteApplication">Type of the concrete application</typeparam>
    /// <typeparam name="TConcreteApplicationAdapter">Type of the concrete application adapter</typeparam>
    public interface IApplicationAdapterFactory<TConcreteApplication, out TConcreteApplicationAdapter>
            where TConcreteApplication : notnull
            where TConcreteApplicationAdapter : notnull, IConcreteApplicationAdapter<TConcreteApplication>
    {
        /// <summary>
        /// Creates a concrete builder adapter.
        /// </summary>
        /// <typeparam name="TBuilder">Type of the framework-agnostic builder abstraction</typeparam>
        /// <typeparam name="TConcreteBuilder">Type of the concrete builder</typeparam>
        /// <returns>A concrete builder adapter</returns>
        IConcreteBuilderAdapter<TConcreteBuilder, TConcreteApplication> CreateConcreteBuilderAdapter<TBuilder, TConcreteBuilder>()
            where TBuilder : IAppBuilderAbstraction
            where TConcreteBuilder : notnull;


        /// <summary>
        /// Creates a concrete application adapter.
        /// </summary>
        /// <param name="concreteApplication">The concrete application instance.</param>
        /// <returns>A concrete application adapter.</returns>
        TConcreteApplicationAdapter CreateApplicationAdapter(TConcreteApplication concreteApplication);


        /// <summary>
        /// Creates a framework-agnostic IApplication instance.
        /// </summary>
        /// <typeparam name="TBuilder">Type of the framework-agnostic builder abstraction</typeparam>
        /// <param name="builder">The framework-agnostic builder abstraction instance.</param>
        /// <param name="concreteApplication">The built concrete application instance (framework-specific).</param>
        /// <returns>A framework-agnostic IApplication instance.</returns>
        IApplication<TBuilder> CreateApplication<TBuilder>(TBuilder builder, TConcreteApplication concreteApplication)
            where TBuilder : IAppBuilderAbstraction;
    }
}
