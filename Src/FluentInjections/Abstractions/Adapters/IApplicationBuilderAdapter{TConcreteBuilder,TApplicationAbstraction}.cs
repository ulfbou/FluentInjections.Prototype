// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    /// <summary>
    /// Represents an adapter for a concrete application builder, providing framework-agnostic configuration.
    /// </summary>
    /// <typeparam name="TConcreteBuilder">The type of the concrete application builder.</typeparam>
    /// <typeparam name="TApplicationAbstraction">The type of the application abstraction.</typeparam>
    public interface IApplicationBuilderAdapter<TConcreteBuilder, TApplicationAbstraction> : IApplicationBuilderAbstraction
        where TApplicationAbstraction : IApplicationAbstraction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IApplicationBuilderAdapter{TConcreteBuilder, TApplicationAbstraction}"/> interface.
        /// </summary>
        /// <param name="concreteBuilder">The concrete application builder instance.</param>
        /// <param name="resolverProvider">The component resolver provider.</param>
        // Constructor: ApplicationBuilderAdapter(TConcreteBuilder concreteBuilder, IComponentResolverProvider resolverProvider);
    }
}
