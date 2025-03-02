// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using FluentInjections.Abstractions.Adapters;

using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Factories
{
    /// <summary>
    /// Represents a factory for creating application builders.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the application builder.</typeparam>
    public interface IBuilderFactory<TBuilder>
        where TBuilder : IApplicationBuilderAbstraction
    {
        // Example constructor: DefaultBuilderFactory(ILoggerFactory loggerFactory, IApplicationAdapterFactory adapterFactory)

        /// <summary>
        /// Creates a new application builder asynchronously.
        /// </summary>
        /// <param name="args">The arguments to pass to the builder.</param>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation, resulting in a new application builder.</returns>
        Task<TBuilder> CreateBuilderAsync(string[]? args = null, CancellationToken cancellationToken = default);
    }
}
