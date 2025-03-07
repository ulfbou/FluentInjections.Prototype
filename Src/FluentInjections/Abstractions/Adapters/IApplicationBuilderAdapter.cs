// Copyright (c) FluentInjections Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace FluentInjections.Abstractions.Adapters
{
    public interface IApplicationBuilderAdapter<TInnerApplication> : IAsyncDisposable
    {
        /// <summary>
        /// Builds the application asynchronously.
        /// </summary>
        Task<IApplicationAdapter<TInnerApplication>> BuildAsync(CancellationToken cancellationToken = default);
    }
}
