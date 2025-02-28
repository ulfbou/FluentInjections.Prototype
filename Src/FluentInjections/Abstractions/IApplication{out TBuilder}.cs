// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions
{
    public interface IApplication<out TBuilder> // 'out' variance for TBuilder? Review if needed
        where TBuilder : IAppBuilderAbstraction
    {
        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the application asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs the application asynchronously. This will typically block until the application is stopped.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to stop the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task RunAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the framework-agnostic builder abstraction that was used to create this application.
        /// </summary>
        TBuilder Builder { get; } // Framework-agnostic builder abstraction
    }
}
