// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.Extensions.DependencyInjection;

namespace FluentInjections.Abstractions.DependencyInjection
{
    /// <summary>
    /// Optionally, defines policies for determining when a new scope should be created
    /// based on the current context and existing scope information.
    /// </summary>
    public interface IContextPolicy
    {
        /// <summary>
        /// Determines whether a new scope should be created given the current context.
        /// </summary>
        /// <param name="currentContextId">The current context identifier.</param>
        /// <param name="existingScope">The existing scope, if any.</param>
        /// <returns>True if a new scope should be created; otherwise, false.</returns>
        bool ShouldCreateNewScope(string currentContextId, IInjectionScope? existingScope);
    }
}
