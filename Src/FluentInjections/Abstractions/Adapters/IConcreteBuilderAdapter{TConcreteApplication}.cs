// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    /// <summary>
    /// Represents a concrete adapter for a framework-specific application builder.
    /// </summary>
    /// <typeparam name="TConcreteApplication">The type of the concrete application instance that will be built.</typeparam>
    public interface IConcreteBuilderAdapter<TConcreteApplication>
    {
        /// <summary>
        /// Builds the concrete application asynchronously using the framework-specific builder.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the built concrete application instance.</returns>
        Task<TConcreteApplication> BuildApplicationAsync(CancellationToken cancellationToken = default);
    }
}
