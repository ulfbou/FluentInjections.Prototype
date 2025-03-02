// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a component registration.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    public interface IComponentRegistration<TComponent, TContract> : IComponentRegistration<TComponent>
        where TComponent : IComponent
    {
        /// <summary>
        /// Gets the instance of the component.
        /// </summary>
        TContract? Instance { get; set; }

        /// <summary>
        /// Gets the factory function to create the component.
        /// </summary>
        Func<IServiceProvider, ValueTask<TContract>> Factory { get; set; }
    }
}
