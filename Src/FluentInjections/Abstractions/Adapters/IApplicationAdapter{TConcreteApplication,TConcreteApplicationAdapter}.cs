// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Extensions.Logging;

namespace FluentInjections.Abstractions.Adapters
{
    /// <summary>
    /// Represents an adapter for a concrete application instance, providing framework-agnostic lifecycle management.
    /// </summary>
    /// <typeparam name="TConcreteApplication">The type of the concrete application instance.</typeparam>
    /// <typeparam name="TConcreteApplicationAdapter">The type of the concrete application adapter.</typeparam>
    public interface IApplicationAdapter<TConcreteApplication, TConcreteApplicationAdapter> : IAsyncDisposable
        where TConcreteApplication : notnull
        where TConcreteApplicationAdapter : IConcreteApplicationAdapter<TConcreteApplication>
    {
        /// <summary>
        /// Gets the concrete application adapter instance.
        /// </summary>
        TConcreteApplicationAdapter Adapter { get; }

        /// <summary>
        /// Gets the logger factory for the application.
        /// </summary>
        ILoggerFactory LoggerFactory { get; }

        /// <summary>
        /// Runs the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RunAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}
