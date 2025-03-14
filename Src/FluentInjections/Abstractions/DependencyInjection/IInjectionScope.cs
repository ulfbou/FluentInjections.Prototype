// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.DependencyInjection
{
    /// <summary>
    /// Represents a single, dynamic injection scope with its own cache and lifecycle.
    /// </summary>
    public interface IInjectionScope : IDisposable
    {
        /// <summary>
        /// A unique identifier for the scope (e.g., request ID, session ID).
        /// </summary>
        string ScopeId { get; }

        /// <summary>
        /// Registers a resolved instance into the scope for caching.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance to cache.</param>
        void RegisterInstance<T>(T instance);

        /// <summary>
        /// Attempts to retrieve an instance from the scope.
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">When this method returns, contains the instance if found; otherwise, default.</param>
        /// <returns>True if an instance was found; otherwise, false.</returns>
        bool TryGetInstance<T>(out T instance);
    }
}
