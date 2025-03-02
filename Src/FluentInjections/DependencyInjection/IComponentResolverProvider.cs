// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions;

namespace FluentInjections.DependencyInjection
{
    /// <summary>
    /// Represents a provider for component resolvers.
    /// </summary>
    public interface IComponentResolverProvider
    {
        /// <summary>
        /// Retrieves a component resolver for the specified component type.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <returns>The component resolver.</returns>
        IComponentResolver<TComponent> GetResolver<TComponent>() where TComponent : Abstractions.IComponent;
    }
}