// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.DependencyInjection
{
    /// <summary>
    /// Resolves components within a given scope, ensuring that scoped instances are reused
    /// and properly managed according to their lifecycle.
    /// </summary>
    public interface IScopedResolver
    {
        /// <summary>
        /// Resolves a component within the current scope. If the component is not already cached,
        /// the provided factory function is invoked to create it.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="alias">Optional alias used to identify the component.</param>
        /// <param name="factory">A function to create the component if not present in the scope.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The resolved component.</returns>
        ValueTask<TContract?> ResolveScopedAsync<TContract>(
            string? alias,
            Func<ValueTask<TContract>> factory,
            CancellationToken cancellationToken = default);
    }
}
