// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents the lifetime of a component.
    /// </summary>
    public enum ComponentLifetime
    {
        /// <summary>
        /// A transient component is created every time it is requested.
        /// </summary>
        Transient,

        /// <summary>
        /// A scoped component is created once per scope.
        /// </summary>
        Scoped,

        /// <summary>
        /// A singleton component is created once per application.
        /// </summary>
        Singleton
    }
}
