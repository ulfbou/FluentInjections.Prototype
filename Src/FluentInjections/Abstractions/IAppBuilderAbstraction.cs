// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a framework-agnostic application builder abstraction.
    /// Implementations are responsible for providing access to the appropriate IApplicationAdapterFactory.
    /// </summary>
    public interface IAppBuilderAbstraction
    {
        /// <summary>
        /// Gets the Type of the IApplicationAdapterFactory to be used with this builder abstraction.
        /// </summary>
        Type AdapterFactoryType { get; }
        /// <summary>
        /// Gets the type of the concrete application built by this abstraction.
        /// </summary>
        Type ConcreteApplicationType { get; }
    }
}
