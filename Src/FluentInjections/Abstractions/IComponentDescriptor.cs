// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.DependencyInjection;

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a compiled component descriptor.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    public interface IComponentDescriptor<TComponent, TContract>
        where TComponent : IComponent
    {
        /// <summary>
        /// Gets the alias of the component descriptor.
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Gets the lifetime of the component.
        /// </summary>
        ComponentLifetime Lifetime { get; }

        /// <summary>
        /// Gets the type of the contract.
        /// </summary>
        Type ContractType { get; }

        /// <summary>
        /// Gets the type of the resolution.
        /// </summary>
        Type? ResolutionType { get; }

        /// <summary>
        /// Gets the instance of the component.
        /// </summary>
        TContract? Instance { get; init; }

        /// <summary>
        /// Gets the factory function to create the component.
        /// </summary>
        Func<IComponentResolver<TComponent>, ValueTask<TContract>>? Factory { get; init; }

        /// <summary>
        /// Gets the configuration action to perform on the component.
        /// </summary>
        Action<TContract>? Configure { get; init; }

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
    }
}
