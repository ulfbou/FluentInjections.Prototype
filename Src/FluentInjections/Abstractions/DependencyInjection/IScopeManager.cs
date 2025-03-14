// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.DependencyInjection
{
    /// <summary>
    /// Manages the creation, retrieval, and disposal of injection scopes based on the current context.
    /// </summary>
    public interface IScopeManager
    {
        /// <summary>
        /// Retrieves the current active scope based on the execution context.
        /// </summary>
        /// <returns>The current <see cref="IInjectionScope"/>.</returns>
        IInjectionScope GetCurrentScope();

        /// <summary>
        /// Creates a new scope for a given context.
        /// </summary>
        /// <param name="scopeId">The unique identifier for the scope.</param>
        /// <returns>The created <see cref="IInjectionScope"/>.</returns>
        IInjectionScope CreateScope(string scopeId);

        /// <summary>
        /// Disposes the scope associated with the specified identifier.
        /// </summary>
        /// <param name="scopeId">The identifier of the scope to dispose.</param>
        void DisposeScope(string scopeId);
    }
}
