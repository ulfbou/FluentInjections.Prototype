// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.DependencyInjection
{
    /// <summary>
    /// Provides the current context identifier (e.g., thread, request, or session ID)
    /// which can be used to determine the correct scope for resolution.
    /// </summary>
    public interface IContextIdentifier
    {
        /// <summary>
        /// Returns a string representing the current execution context.
        /// </summary>
        /// <returns>The current context identifier.</returns>
        string GetCurrentContextId();
    }
}
