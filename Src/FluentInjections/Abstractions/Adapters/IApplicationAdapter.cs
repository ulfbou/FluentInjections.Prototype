// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationAdapter : IAsyncDisposable
    {
        /// <summary>
        /// Starts the application asynchronously.
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops the application asynchronously.
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Runs the application asynchronously.
        /// </summary>
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
