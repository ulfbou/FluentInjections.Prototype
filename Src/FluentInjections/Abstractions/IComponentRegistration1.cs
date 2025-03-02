// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Diagnostics.Contracts;

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a component registration.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    /// <typeparam name="TContract">The type of the contract.</typeparam>
    public interface IComponentRegistration<TComponent> where TComponent : IComponent
    {
        /// <summary>
        /// Gets the component alias.
        /// </summary>
        string Alias { get; set; }

        /// <summary>
        /// Gets the component lifetime.
        /// </summary>
        ComponentLifetime Lifetime { get; set; }

        /// <summary>
        /// Gets the component contract type.
        /// </summary>
        Type ContractType { get; set; }

        /// <summary>
        /// Gets the component resolution type.
        /// </summary>
        Type? ResolutionType { get; set; }

        /// <summary>
        /// Gets the component metadata.
        /// </summary>
        IDictionary<string, object?> Metadata { get; set; }

        /// <summary>
        /// Gets the component parameters.
        /// </summary>
        object Parameters { get; set; }

        /// <summary>
        /// Gets the condition to determine if the component is valid.
        /// </summary>
        Func<IServiceProvider, CancellationToken, ValueTask<bool>>? Condition { get; set; }
    }
}
