// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a component registration.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    public interface IComponentRegistration<TComponent, TContract> : IComponentRegistration<TComponent>, IDisposable
        where TComponent : IComponent
    {
        /// <summary>
        /// Gets the instance of the component.
        /// </summary>
        TContract? Instance { get; set; }

        /// <summary>
        /// Gets the factory function to create the component.
        /// </summary>
        Func<IComponentResolver<TComponent>, CancellationToken, ValueTask<TContract>>? Factory { get; set; }

        /// <summary>
        /// Gets or Sets the configuration asynchronous action to perform on the component.
        /// </summary>
        Func<TContract, CancellationToken, ValueTask>? Configure { get; set; }

        /// <summary>
        /// Creates a component descriptor from the registration.
        /// </summary>
        /// <param name="alias">The alias of the component descriptor.</param>
        /// <returns>The component descriptor.</returns>
        IComponentDescriptor<TComponent, TContract> CreateDescriptor(string alias);
    }
}
