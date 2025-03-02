// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a component registration.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    public interface IComponentRegistration<TComponent, TContract>
        where TComponent : IComponent
    {
        /// <summary>
        /// Gets the type of the contract.
        /// </summary>
        Type ContractType { get; }

        /// <summary>
        /// Gets the type of the resolution.
        /// </summary>
        Type ResolutionType { get; }

        /// <summary>
        /// Gets the instance of the component.
        /// </summary>
        TContract? Instance { get; }

        /// <summary>
        /// Gets the factory function to create the component.
        /// </summary>
        Func<IServiceProvider, ValueTask<TContract>> Factory { get; }

        /// <summary>
        /// Gets the lifetime of the component.
        /// </summary>
        ComponentLifetime Lifetime { get; }

        /// <summary>
        /// Gets the metadata of the component.
        /// </summary>
        IReadOnlyDictionary<string, object?> Metadata { get; }

        /// <summary>
        /// Gets the parameters of the component.
        /// </summary>
        object Parameters { get; }

        /// <summary>
        /// Gets the condition to determine if the component is valid.
        /// </summary>
        Func<IServiceProvider, ValueTask<bool>>? Condition { get; }

        /// <summary>
        /// Creates a component descriptor from the registration.
        /// </summary>
        /// <param name="alias">The alias of the component descriptor.</param>
        /// <returns>The component descriptor.</returns>
        IComponentDescriptor<TComponent, TContract> CreateDescriptor(string alias);
    }
}
