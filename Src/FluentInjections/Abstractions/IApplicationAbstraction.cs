// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    /// <summary>
    /// Represents a framework-agnostic application abstraction.
    /// </summary>
    public interface IApplicationAbstraction : IAsyncDisposable
    {
        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs the application asynchronously. This will typically block until the application is stopped.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
